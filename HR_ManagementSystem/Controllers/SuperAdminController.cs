using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;
using HR_ManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perm = HR_ManagementSystem.Utilities.Permission;
namespace HR_ManagementSystem.Controllers
{
    [Route("api/[controller]")]
  [Authorize(Roles = "SuperAdmin")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SuperAdminController(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Perm.SuperAdmin.View)]
        [HttpGet("GetUsers")]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRoles = new List<Object>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new {user.Id , user.FullName , user.Email , Roles = roles});
            }
            return Ok(usersWithRoles);
        }
        [Authorize(Perm.SuperAdmin.View)]
        [HttpGet("UserRoles")]
        public async Task<ActionResult> ManageRoles (string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound("User Not Found");
            }
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoleVM = new UserRolesDTO()
            {
                UserId = userId,
                UserEmail = user.Email,
                Roles = roles.Select(role => new CheckBoxDTO
                {
                    DisplayValue = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user,role.Name).Result
                }).ToList()
            };
            return Ok(userRoleVM);
        }

        [Authorize(Perm.SuperAdmin.Edit)]
        [HttpPut("UpdateRoles")]
        public async Task<ActionResult> UpdateRoles(UserRolesDTO userRolesDTO)
        {
            var user = await _userManager.FindByIdAsync(userRolesDTO.UserId);
            if (user == null)
                return NotFound("User Not Found");

            var currentRoles = await _userManager.GetRolesAsync(user);

            
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

          
            var selectedRoles = userRolesDTO.Roles
                .Where(r => r.IsSelected)
                .Select(r => r.DisplayValue)
                .ToList();

            await _userManager.AddToRolesAsync(user, selectedRoles);

            return Ok(new
            {
                Message = "Roles updated successfully",
                user.Email,
                UpdatedRoles = selectedRoles
            });
        }
        [Authorize(Perm.SuperAdmin.Edit)]
        [HttpPut("UpdateRole")]
        public async Task<ActionResult> UpdateRole(string roleId, RoleFormDTO model)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound("Role Not Found");

            if (role.Name.ToLower() != model.Name.Trim().ToLower() &&
                await _roleManager.RoleExistsAsync(model.Name.Trim()))
            {
                return BadRequest("This role name already exists");
            }

            role.Name = model.Name.Trim();
            role.NormalizedName = model.Name.Trim().ToUpper();

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new
            {
                Message = "Role updated successfully",
                UpdatedRole = role
            });
        }
        [Authorize(Perm.SuperAdmin.View)]
        [HttpGet("AllRoles")]
        public async Task <ActionResult> GetAllRoles ()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
        [Authorize(Perm.SuperAdmin.Create)]
        [HttpPost("AddRole")]
        public async Task<ActionResult> Add(RoleFormDTO model)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetAllRoles));
            }
            if(await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Error", "Role Already Exists!");
                return RedirectToAction(nameof(GetAllRoles));
            }
            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
            var role = await _roleManager.FindByNameAsync(model.Name.Trim());
            return CreatedAtAction("GetRoleById", role.Id, role);
        }
        [Authorize(Perm.SuperAdmin.View)]
        [HttpGet("GetRoleById")]
        public async Task<ActionResult> GetRoleById (string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role == null)
                return NotFound("No role found with this ID");
            return Ok(role);
        }
        [Authorize(Perm.SuperAdmin.Delete)]
        [HttpDelete("DeleteRole")]
        public async Task<ActionResult> DeleteRole (string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound("No role found with this ID");
            var isAdminRole = role.Name.Equals(Roles.Admin.ToString(), StringComparison.OrdinalIgnoreCase);
            var isSuperAdminRole = role.Name.Equals(Roles.SuperAdmin.ToString(), StringComparison.OrdinalIgnoreCase);
            if(isAdminRole || isSuperAdminRole)
                return BadRequest("Not Allowed To Delete SuperAdmin Role Or Admin Role");
            await _roleManager.DeleteAsync(role);
            
            return NoContent();
        }
        [Authorize(Perm.SuperAdmin.View)]
        [HttpGet("AllPermissions")]
        public async Task<ActionResult> ManagePermissions (string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound("No role found with this ID");
            var roleClaims =  _roleManager.GetClaimsAsync(role).Result.Select(c=>c.Value).ToList();
            var allClaims = Utilities.Permission.PermissionsList();
            var allPermissions = allClaims.Select(p => new CheckBoxDTO
            {
                DisplayValue = p
            }).ToList();
            foreach(var permision in allPermissions)
                if(roleClaims.Any(c=>c == permision.DisplayValue))
                    permision.IsSelected = true;

            var permissionDTO = new PermissionFormDTO
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleClaims = allPermissions
            };
            return Ok(permissionDTO);
        }
        [Authorize(Perm.SuperAdmin.Create)]
        [HttpPost("AddPermission")]
        public async Task<ActionResult> AddPermissions(PermissionFormDTO model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
                return NotFound("Role Not Found");
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var roleClaim in roleClaims)
                await _roleManager.RemoveClaimAsync(role, roleClaim);
            var selectedClaims = model.RoleClaims.Where(c=>c.IsSelected).ToList();
            foreach (var claim in selectedClaims)
                await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(Constants.Permission, claim.DisplayValue));
            return CreatedAtAction(nameof(ManagePermissions), role.Id);
        }





        [Authorize(Perm.SuperAdmin.Delete)]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser (string userId)
        {
            var user = await _userManager.FindByIdAsync (userId);
            if(user == null)
            {
                return NotFound("User Not Found");
            }
            var isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin.ToString());
            var isSuper = await _userManager.IsInRoleAsync(user, Roles.SuperAdmin.ToString());
            if (isSuper)
                return BadRequest("Not Allowed To Delete Super Admin");

            if(isAdmin && User.IsInRole(Roles.Admin.ToString()))
            {
                if(user != null && user.FullName.Trim().ToLower() == "admin")
                
                    return BadRequest("Not Allowed To Delete Built-In Admin"); 
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

    }
}
