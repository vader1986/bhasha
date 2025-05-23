using Azure.Storage.Blobs;
using Bhasha;
using Bhasha.Areas.Identity;
using Bhasha.Domain.Interfaces;
using Bhasha.Identity;
using Bhasha.Identity.Extensions;
using Bhasha.Infrastructure.AzureBlob;
using Bhasha.Infrastructure.AzureSpeechApi;
using Bhasha.Infrastructure.AzureTranslatorApi;
using Bhasha.Infrastructure.BlazorSpeechSynthesis;
using Bhasha.Infrastructure.EntityFramework;
using Bhasha.Infrastructure.FileSystem;
using Bhasha.Infrastructure.Toolbelt;
using Bhasha.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;

try
{
    var builder = WebApplication
        .CreateBuilder(args);

    ////////////////////
    // Service Configuration
    ////////////////////
    
    builder
        .Configuration
        .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);

    var translationConfiguration = new TranslationConfiguration();
    
    builder.Configuration
        .GetSection(TranslationConfiguration.SectionName)
        .Bind(translationConfiguration);
    
    var resources = new ResourcesSettings();
    
    builder.Configuration
        .GetSection(ResourcesSettings.SectionName)
        .Bind(resources);
    
    ////////////////////
    // DB & Identity
    ////////////////////
    
    var services = builder.Services;
    var connectionString = builder.Configuration.GetConnectionString("postgres");

    services.AddAuthentication();
    services.AddAuthorizationBuilder();
    services.AddDbContext<AppDbContext>(db => db.UseNpgsql(connectionString), ServiceLifetime.Transient); 

    services.AddScoped<IChapterRepository, EntityFrameworkChapterRepository>();
    services.AddScoped<IExpressionRepository, EntityFrameworkExpressionRepository>();
    services.AddScoped<IProfileRepository, EntityFrameworkProfileRepository>();
    services.AddScoped<ITranslationRepository, EntityFrameworkTranslationRepository>();
    services.AddScoped<IStudyCardRepository, EntityFrameworkStudyCardRepository>();
    
    services
        .AddIdentity<AppUser, AppRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddApiEndpoints()
        .AddDefaultTokenProviders()
        .AddDefaultUI();
    
    services
        .AddSingleton<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>();

    ////////////////////
    // Blazor
    ////////////////////
    
    services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/Web/Pages");
    services.AddRazorPages();
    services.AddServerSideBlazor(x =>
    {
        x.DetailedErrors = true;
    });
    services.AddMudServices(x =>
    {
        x.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
        x.SnackbarConfiguration.VisibleStateDuration = 500;
    });

    ////////////////////
    // Application
    ////////////////////

    services.AddHealthChecks();
    services.AddScoped<IValidator, Validator>();
    services.AddScoped<IChapterSummariesProvider, ChapterSummariesProvider>();
    services.AddScoped<IChapterProvider, ChapterProvider>();
    services.AddScoped<IAuthoringService, AuthoringService>();
    services.AddScoped<IStudyingService, StudyingService>();
    services.AddSingleton<ITranslationProvider, CachingTranslationProvider>();

    // resource management
    services.AddSingleton(resources);

    var azureBlobConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

    if (string.IsNullOrWhiteSpace(azureBlobConnectionString))
    {
        services.AddSingleton<IResourcesManager, FileSystemResourcesManager>();
    }
    else
    {
        services.AddSingleton(new BlobServiceClient(connectionString: azureBlobConnectionString));
        services.AddSingleton<IResourcesManager, AzureBlobResourcesManager>();
    }
    
    if (translationConfiguration.AzureTranslatorApi is null)
    {
        services.AddSingleton<ITranslator, NoTranslator>();
    }
    else
    {
        services.AddSingleton(translationConfiguration.AzureTranslatorApi);
        services.AddSingleton<ITranslator, AzureTranslatorApiClient>();
    }

    switch (translationConfiguration.Provider)
    {
        case TranslationProvider.Azure when translationConfiguration.AzureSpeechApi is not null:
            services.AddSingleton(translationConfiguration.AzureSpeechApi);
            services.AddScoped<ISpeaker, AzureSpeaker>();
            break;
        case TranslationProvider.Toolbelt:
            services.AddSpeechSynthesis();
            services.AddScoped<ISpeaker, ToolbeltSpeaker>();
            break;
        case TranslationProvider.BlazorSpeechSynthesis:
            services.AddSpeechSynthesisServices();
            services.AddScoped<ISpeaker, BlazorSpeaker>();
            break;
        default:
            services.AddSpeechSynthesisServices();
            services.AddScoped<ISpeaker, BlazorSpeaker>();
            break;
    }
    
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

    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    
    var identitySettings = IdentitySettings.From(app.Configuration);
    var scope = ((IApplicationBuilder)app).ApplicationServices.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    
    await serviceProvider.UseIdentitySettings(identitySettings);

    var context = serviceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    
    app.MapIdentityApi<AppUser>();
    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}