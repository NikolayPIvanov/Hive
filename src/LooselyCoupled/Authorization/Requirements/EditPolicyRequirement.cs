using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hive.Common.Core.SeedWork;
using Microsoft.AspNetCore.Authorization;

namespace Hive.LooselyCoupled.Authorization.Requirements
{
    public class EntityOwnerAuthorizationHandler : 
        AuthorizationHandler<OnlyOwnerAuthorizationRequirement, Entity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OnlyOwnerAuthorizationRequirement requirement,
            Entity resource)
        {
            var roles = context.User?.FindAll(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).ToList();
            if (requirement.AllowAdmin)
            {
                var isAdmin = roles.Any(r => r.Value == "Admin");
                if (isAdmin)
                {
                    context.Succeed(requirement);
                }
            }
            
            if (context.User?.FindFirstValue(ClaimTypes.NameIdentifier) == resource.CreatedBy)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public record OnlyOwnerAuthorizationRequirement(bool AllowAdmin = true) : IAuthorizationRequirement { }
}