using Bhasha.Shared.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bhasha.Identity.Extensions;

public static class ServiceProviderExtensions
{
    private static async Task UseRoles(this IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
        var roles = new[]
        {
            new AppRole{ Name = Roles.Admin, Id = Roles.Admin },
            new AppRole{ Name = Roles.Author, Id = Roles.Author },
            new AppRole{ Name = Roles.Student, Id = Roles.Student }
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
                Id = Guid.NewGuid().ToString(),
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
    
    public static async Task UseIdentitySettings(this IServiceProvider serviceProvider, IdentitySettings settings)
    {
        await serviceProvider.UseRoles();
        await serviceProvider.UseDefaultUsers(settings);
    }
}