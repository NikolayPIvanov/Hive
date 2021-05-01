using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hive.Common.Core.Security.Requirements;
using Hive.Common.Core.SeedWork;
using Microsoft.AspNetCore.Authorization;

namespace Hive.Common.Core.Security.Handlers
{
    public class EntityOwnerAuthorizationHandler : 
        AuthorizationHandler<OnlyOwnerAuthorizationRequirement, Entity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OnlyOwnerAuthorizationRequirement requirement,
            Entity resource)
        {
            var roles = context.User?.FindAll(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).ToList();
            if (requirement.SuperRoles.Any())
            {
                var isExplicitlyAllowed = roles.Select(r => r.Value).Intersect(requirement.SuperRoles).Any();
                if (isExplicitlyAllowed)
                {
                    context.Succeed(requirement);
                }
            }
            
            if (context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value == resource.CreatedBy)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}