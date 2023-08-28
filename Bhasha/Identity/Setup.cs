using Microsoft.AspNetCore.Identity;

namespace Bhasha.Identity;

public static class Setup
{
	private static async Task AddRoles(this IServiceProvider serviceProvider)
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

	private static async Task AddDefaultUsers(this IServiceProvider serviceProvider, IConfiguration config)
	{
		var settings = IdentitySettings.From(config);
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

	public static async Task UseDefaultIdentitySetup(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();

		await scope.ServiceProvider.AddRoles();
		await scope.ServiceProvider.AddDefaultUsers(app.Configuration);
	}
}

