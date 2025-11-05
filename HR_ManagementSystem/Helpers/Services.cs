using HR_ManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HR_ManagementSystem.Helpers
{
    public class Services
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;

        public Services(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<string>> GetRolesForUserAsync(ClaimsPrincipal user)
        {
            var applicationUser = await _userManager.GetUserAsync(user);
            if(applicationUser == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(applicationUser);
            return roles.ToList();

        }
        public async Task<List<string>> GetRolesForUserByIdAsync (string userId)
        {
            var applicationUser = await _userManager.FindByIdAsync(userId);
            if(applicationUser == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(applicationUser);
            return roles.ToList();
        }
        public async Task<List<string>> GetRolesForUserByNameAsync(string userName)
        {
            var applicationUser = await _userManager.FindByNameAsync(userName);
            if (applicationUser == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(applicationUser);
            return roles.ToList();
        }
    }
}
