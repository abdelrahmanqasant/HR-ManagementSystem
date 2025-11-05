using HR_ManagementSystem.DTOs;
using HR_ManagementSystem.Models;
using static HR_ManagementSystem.Utilities.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Permissions;
using System.Text;

namespace HR_ManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public ApplicationUserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [Authorize(ApplicationUserPerm.Create)]
        [HttpPost("Register")]
    
        public async Task<ActionResult> Registeration(RegisterDTO account)
        {
            var existingUser = await _userManager.FindByEmailAsync(account.Email);
            if (existingUser != null)
                return BadRequest(new { message = "User with this email already exists" });

            if (!await _roleManager.RoleExistsAsync(account.RoleName))
                return NotFound(new { message = "Role Not Found" });

            ApplicationUser user = new ApplicationUser()
            {
                FullName = account.FullName,
                Email = account.Email,
                UserName = account.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, account.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, account.RoleName);
                return Created("", new { message = "User created successfully" });
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { errors });
        }

    
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginDTO account)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(account.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            bool checkPassword = await _userManager.CheckPasswordAsync(user, account.Password);
            if (!checkPassword)
                return Unauthorized(new { message = "Invalid credentials" });

         
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

           
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                var roleEntity = await _roleManager.FindByNameAsync(role);
                if (roleEntity != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                    foreach (var roleClaim in roleClaims.Where(c => c.Type == "Permission"))
                    {
                        claims.Add(new Claim("Permission", roleClaim.Value));
                    }
                }
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                fullName = user.FullName,
                role = roles.Count > 0 ? roles[0] : null,
                permissions = claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList()
            });
        }

       
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            try
            {
                

                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during logout", details = ex.Message });
            }
        }
    }
}
