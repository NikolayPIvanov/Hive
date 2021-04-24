using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Hive.Gig.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpContext? _httpContext;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }
        
        public Task<bool> IsInRoleAsync(string userId, string role)
        {
            var roleClaim = _httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType);
            if (roleClaim == null)
            {
                return Task.FromResult<bool>(false);
            }

            var isInRole = roleClaim.Value.Contains(role);

            return Task.FromResult(isInRole);
        }

        public Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            throw new System.NotImplementedException();
        }
    }
}