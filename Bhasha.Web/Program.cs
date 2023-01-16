using AspNetCore.Identity.Mongo;
using Bhasha.Web;
using Bhasha.Web.Areas.Identity;
using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Interfaces;
using Bhasha.Web.Domain.Pages;
using Bhasha.Web.Identity;
using Bhasha.Web.Infrastructure.Mongo;
using Bhasha.Web.Services;
using Bhasha.Web.Services.Pages;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.DotNetCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MudBlazor.Services;
using Orleans.Configuration;
using Orleans.Providers;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);

    ////////////////////
    // Orleans
    ////////////////////

    var hostBuilder = builder.Host.UseOrleans(siloBuilder =>
    {
        siloBuilder
            .UseLocalhostClustering()
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "bhasha";
            })
            .ConfigureLogging(logging => logging.AddConsole())
            .AddMemoryGrainStorage(Bhasha.Web.Orleans.StorageProvider)
            .AddMemoryStreams(Bhasha.Web.Orleans.StreamProvider);
    });

    var mongoSettings = MongoSettings.From(builder.Configuration);

    BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
    BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

    hostBuilder.ConfigureServices(services =>
    {
        ////////////////////
        // Identity Server
        ////////////////////

        services
            .AddIdentityMongoDbProvider<AppUser, AppRole, Guid>(
            identity => {
                // identity server settings
            },
            mongo => {
                mongo.ConnectionString = mongoSettings.ConnectionString;
            });

        services
            .AddDatabaseDeveloperPageExceptionFilter();

        services
            .AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddDefaultUI()
            .AddRoles<AppRole>()
            .AddMongoDbStores<AppUser, AppRole, Guid>(mongo =>
            {
                mongo.ConnectionString = mongoSettings.ConnectionString;
            })
            .AddDefaultTokenProviders();

        ////////////////////
        // Blazor
        ////////////////////

        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddSingleton<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>();
        services.AddMudServices();

        ////////////////////
        // MongoDB
        ////////////////////

        services.AddSingleton(mongoSettings);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));
        services.AddMigration(new MongoMigrationSettings
        {
            Database = mongoSettings.DatabaseName,
            ConnectionString = mongoSettings.ConnectionString
        });

        ////////////////////
        // Application
        ////////////////////

        services.AddSingleton<ITranslationRepository, MongoTranslationRepository>();
        services.AddSingleton<IChapterRepository, MongoChapterRepository>();
        services.AddSingleton<IExpressionRepository, MongoExpressionRepository>();
        services.AddSingleton<IProfileRepository, MongoProfileRepository>();
        services.AddSingleton<IFactory<Expression>, ExpressionFactory>();
        services.AddSingleton<IValidator, Validator>();
        services.AddSingleton<IAsyncFactory<Page, LangKey, DisplayedPage>, PageFactory>();
        services.AddSingleton<IAsyncFactory<Page, LangKey, DisplayedPage<MultipleChoice>>, MultipleChoicePageFactory>();

    });

    var app = builder.Build();

    ////////////////////
    // HTTP pipeline
    ////////////////////

    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    await app.UseDefaultIdentitySetup();

    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

