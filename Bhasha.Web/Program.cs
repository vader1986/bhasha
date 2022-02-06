using AspNetCore.Identity.Mongo;
using Bhasha.Web;
using Bhasha.Web.Areas.Identity;
using Bhasha.Web.Domain;
using Bhasha.Web.Identity;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Mongo;
using Bhasha.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.DotNetCore;
using MongoDB.Driver;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
var mongoSettings = MongoSettings.From(builder.Configuration);

builder.Services
    .AddIdentityMongoDbProvider<AppUser, AppRole, Guid>(
    identity => {
        // identity server settings
    },
    mongo => {
        mongo.ConnectionString = mongoSettings.ConnectionString;
    });

builder.Services
    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddRoles<AppRole>()
    .AddMongoDbStores<AppUser, AppRole, Guid>(mongo =>
    {
        mongo.ConnectionString = mongoSettings.ConnectionString;
    })
    .AddDefaultTokenProviders();;

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>();
builder.Services.AddMudServices();

// MongoDB
builder.Services.AddSingleton(mongoSettings);
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));
builder.Services.AddMigration(new MongoMigrationSettings
{
    Database = mongoSettings.DatabaseName,
    ConnectionString = mongoSettings.ConnectionString
});

// Bhasha services
builder.Services.AddSingleton<IRepository<Profile>, MongoRepository<Profile>>();
builder.Services.AddSingleton<IRepository<Chapter>, MongoRepository<Chapter>>();
builder.Services.AddSingleton<IRepository<Expression>, MongoRepository<Expression>>();
builder.Services.AddSingleton<IFactory<Profile>, ProfileFactory>();
builder.Services.AddSingleton<IProfileManager, ProfileManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

