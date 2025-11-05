using HR_ManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;

namespace HR_ManagementSystem.Permission
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler()
        {
            
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                            PermissionRequirement requirement)
        {
            if (context.User == null)
                return;

          
            Console.WriteLine($" Checking permission: {requirement.Permission}");

            var permissionClaims = context.User.Claims.Where(x => x.Type == "Permission");
            Console.WriteLine($" Found {permissionClaims.Count()} permission claims");

            foreach (var claim in permissionClaims)
            {
                Console.WriteLine($"   Permission: {claim.Value}");
            }

            var hasPermission = context.User.Claims.Any(x =>
                x.Type == "Permission" &&
                x.Value == requirement.Permission
            );

            if (hasPermission)
            {
                Console.WriteLine($" Permission GRANTED: {requirement.Permission}");
                context.Succeed(requirement);
            }
            else
            {
                Console.WriteLine($" Permission DENIED: {requirement.Permission}");
            }
        }
    }
}
