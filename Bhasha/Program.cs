using Bhasha.Areas.Identity;
using Bhasha.Domain.Interfaces;
using Bhasha.MongoDb.Extensions;
using Bhasha.MongoDb.Identity;
using Bhasha.MongoDb.Infrastructure.Mongo;
using Bhasha.Services;
using Bhasha.Services.Pages;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MudBlazor.Services;

try
{
    var builder = WebApplication
        .CreateBuilder(args);

    builder
        .Configuration
        .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);

    MongoSetup.Configure();

    var services = builder.Services;
    services
        .AddMongoDb(builder.Configuration)
        .AddMongoDbIdentityServer(builder.Configuration)
        .AddDefaultUI();
    
    ////////////////////
    // Blazor
    ////////////////////

    services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/Web/Pages");
    services.AddRazorPages();
    services.AddServerSideBlazor();
    services.AddSingleton<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>();
    services.AddMudServices();

    ////////////////////
    // Application
    ////////////////////

    services.AddSingleton<IFactory<Expression>, ExpressionFactory>();
    services.AddSingleton<IValidator, Validator>();
    services.AddSingleton<IPageFactory, PageFactory>();
    services.AddSingleton<IMultipleChoicePageFactory, MultipleChoicePageFactory>();
    services.AddSingleton<IChapterSummariesProvider, ChapterSummariesProvider>();

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

    var identitySettings = IdentitySettings.From(app.Configuration);
    
    await app.UseMongoDbSetup(identitySettings);

    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

