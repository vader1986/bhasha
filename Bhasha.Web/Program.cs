using AspNetCore.Identity.Mongo;
using Bhasha.Web;
using Bhasha.Web.Areas.Identity;
using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Pages;
using Bhasha.Web.Grains;
using Bhasha.Web.Identity;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Mongo;
using Bhasha.Web.Services;
using Bhasha.Web.Services.Pages;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.DotNetCore;
using MongoDB.Driver;
using MudBlazor.Services;

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(c =>
{
    c.AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);
});

var hostBuilder = builder.Host.UseOrleans(siloBuilder =>
{
    ////////////////////
    // Orleans
    ////////////////////

    siloBuilder
        .UseLocalhostClustering()
        .Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "dev";
            options.ServiceId = "bhasha";
        })
        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(FakeGrain).Assembly).WithReferences())
        .ConfigureLogging(logging => logging.AddConsole())
        .AddSimpleMessageStreamProvider("SMSProvider")
        .AddMemoryGrainStorage("PubSubStore");
});

var mongoSettings = MongoSettings.From(builder.Configuration);

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

    services.AddSingleton<IRepository<Profile>, MongoRepository<Profile>>();
    services.AddSingleton<IRepository<Chapter>, MongoRepository<Chapter>>();
    services.AddSingleton<IRepository<Expression>, MongoRepository<Expression>>();
    services.AddSingleton<IRepository<Translation>, MongoRepository<Translation>>();
    services.AddSingleton<IFactory<Expression>, ExpressionFactory>();
    services.AddSingleton<IProfileManager, ProfileManager>();
    services.AddSingleton<IProfileRepository, ProfileRepository>();
    services.AddSingleton<IProgressManager, ProgressManager>();
    services.AddSingleton<ISubmissionManager, SubmissionManager>();
    services.AddSingleton<ITranslationManager, TranslationManager>();
    services.AddSingleton<ITranslationProvider, TranslationProvider>();
    services.AddSingleton<IChapterProvider, ChapterProvider>();
    services.AddSingleton<IChapterLookup, ChapterLookup>();
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