using CareerPath.Identity.Core.Entities;
using CareerPath.Identity.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace CareerPath.Identity.Infrastructure.Persistence;

public static class IdentityDataSeeder
{
    public static async Task SeedAsync(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration)
    {
        // 1. Seed Roles dynamically from the C# Enum
        foreach (var roleName in Enum.GetNames(typeof(Roles)))
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role(roleName));
            }
        }

        // 2. Seed Super Admin User from Configuration (e.g., appsettings.json)
        var superAdminEmail = configuration["SuperAdmin:Email"];
        var superAdminPassword = configuration["SuperAdmin:Password"];

        // Only proceed if the configuration values actually exist
        if (!string.IsNullOrWhiteSpace(superAdminEmail) && !string.IsNullOrWhiteSpace(superAdminPassword))
        {
            if (await userManager.FindByEmailAsync(superAdminEmail) == null)
            {
                var superAdmin = new User
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    FirstName = "System",
                    LastName = "SuperAdmin",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(superAdmin, superAdminPassword);

                if (result.Succeeded)
                {
                    // Assign the SuperAdmin role using the Enum for safety
                    await userManager.AddToRoleAsync(superAdmin, Roles.SuperAdmin.ToString());
                }
            }
        }
    }
}