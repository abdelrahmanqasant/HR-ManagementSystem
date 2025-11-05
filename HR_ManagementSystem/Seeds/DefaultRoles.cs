using HR_ManagementSystem.Utilities;
using Microsoft.AspNetCore.Identity;

namespace HR_ManagementSystem.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync (RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.ApplicationUser.ToString()));
            }
        }
    }
}
