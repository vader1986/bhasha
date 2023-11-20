using AspNetCore.Identity.Mongo;
using Bhasha.MongoDb.Identity;
using Bhasha.MongoDb.Infrastructure.Mongo;
using Bhasha.Shared.Domain.Interfaces;
using Bhasha.Shared.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.DotNetCore;
using MongoDB.Driver;

namespace Bhasha.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
	{
		var settings = MongoSettings.From(configuration);

		services.AddSingleton(settings);
		services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));
		services.AddMigration(new MongoMigrationSettings
		{
			Database = settings.DatabaseName,
			ConnectionString = settings.ConnectionString
		});
		services.AddMongoDbRepositories();
		
		return services;
	}
	
	public static IdentityBuilder AddMongoDbIdentityServer(this IServiceCollection services, IConfiguration configuration)
	{
		var settings = MongoSettings.From(configuration);

		services
			.AddIdentityMongoDbProvider<AppUser, AppRole, Guid>(
				_ => {
					// identity server settings
				},
				mongo => {
					mongo.ConnectionString = settings.ConnectionString;
				});

		var identityBuilder = services
			.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddRoles<AppRole>();

		return identityBuilder
			.AddMongoDbStores<AppUser, AppRole, Guid>(mongo =>
			{
				mongo.ConnectionString = settings.ConnectionString;
			})
			.AddDefaultTokenProviders();
	}
	
    private static void AddMongoDbRepositories(this IServiceCollection services)
    {
        services.AddSingleton<ITranslationRepository, MongoTranslationRepository>();
        services.AddSingleton<IChapterRepository, MongoChapterRepository>();
        services.AddSingleton<IExpressionRepository, MongoExpressionRepository>();
        services.AddSingleton<IProfileRepository, MongoProfileRepository>();
    }

	private static async Task UseRoles(this IServiceProvider serviceProvider)
	{
		var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
		var roles = new[]
		{
			new AppRole{ Name = Roles.Admin, Id = Guid.NewGuid() },
			new AppRole{ Name = Roles.Author, Id = Guid.NewGuid() },
			new AppRole{ Name = Roles.Student, Id = Guid.NewGuid() }
		};

		foreach (var role in roles)
		{
			if (role.Name != null)
			{
                if (await roleManager.RoleExistsAsync(role.Name) == false)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }				
	}

	private static async Task UseDefaultUsers(this IServiceProvider serviceProvider, IdentitySettings settings)
	{
		var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

		foreach (var defaultUser in settings.DefaultUsers)
	    {
			var user = await userManager.FindByEmailAsync(defaultUser.Email);

			if (user != null)
				continue;

			user = new AppUser
			{
				Id = Guid.NewGuid(),
				Email = defaultUser.Email,
				EmailConfirmed = true,
				UserName = defaultUser.Email
			};

			var result = await userManager.CreateAsync(user, defaultUser.Password);
			if (!result.Succeeded)
			{
				throw new InvalidOperationException($"Failed to create default user {user.Email}: {result}");
			}

			result = await userManager.AddToRoleAsync(user, defaultUser.Role);
			if (!result.Succeeded)
			{
				throw new InvalidOperationException($"Failed to add role {defaultUser.Role} to default user {user.Email}: {result}");
			}
		}
	}

	public static async Task UseMongoDbSetup(this IApplicationBuilder builder, IdentitySettings settings)
	{
		using var scope = builder.ApplicationServices.CreateScope();

		await scope.ServiceProvider.UseRoles();
		await scope.ServiceProvider.UseDefaultUsers(settings);
	}
}