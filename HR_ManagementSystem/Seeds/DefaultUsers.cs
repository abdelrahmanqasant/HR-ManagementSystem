using HR_ManagementSystem.Models;
using static  HR_ManagementSystem.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using HR_ManagementSystem.Utilities;


namespace HR_ManagementSystem.Seeds
{
    public static class DefaultUsers
    { 
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var DefaultUser = new ApplicationUser()
            {
                Email = SuperAdminEmail,
                UserName = SuperAdminUserName,
                FullName = SuperAdminFullName,
                EmailConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(DefaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(DefaultUser, SuperAdminPassword);
                await userManager.AddToRoleAsync(DefaultUser, Roles.SuperAdmin.ToString());
            }
            await roleManager.SeedClaimsAsync();

        }
        public static async Task SeedAdminUsersAsync (UserManager<ApplicationUser> userManager,RoleManager < IdentityRole > roleManager)
        {
            var DefaultUser = new ApplicationUser()
            {
                Email = AdminEmail,
                UserName = AdminUserName,
                         
                FullName = AdminFullName,
                EmailConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(DefaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(DefaultUser, AdminPassword);
                await userManager.AddToRoleAsync(DefaultUser, Roles.Admin.ToString());
            }
        }



        public static async Task SeedBasicUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var DefaultUser = new ApplicationUser()
            {
                Email = BasicEmail,
                UserName = BasicUserName,
                FullName = BasicFullName,
                EmailConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(DefaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(DefaultUser, BasicPassword);
                await userManager.AddToRoleAsync(DefaultUser, Roles.ApplicationUser.ToString());
            }
        }







        public static async Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var superAdmin = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            foreach(var module in Enum.GetValues(typeof(PermissionModuleName)))
            
                await roleManager. AddPermissionClaims(superAdmin , module.ToString());
            
        }
      

        public static async Task SeedClaimsAsync (this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            var modules = Enum.GetValues(typeof(PermissionModuleName));
            foreach (var module in modules)
            
              await roleManager.AddPermissionClaims(adminRole, module.ToString());
            
        }
        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager,IdentityRole role,string module)
        {
            var allClaims = await roleManager.GetClaimsAsync (role);    
            var allPermissions = Utilities.Permission.GeneratePermissionsFromModule(module); 
            foreach (var permission  in allPermissions)
            
                if(! allClaims.Any(x => x.Type == Constants.Permission && x.Value == permission))
                
                    await roleManager.AddClaimAsync(role, new Claim(Constants.Permission, permission));
                
            
        }

















                                           }
}
