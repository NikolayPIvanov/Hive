using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hive.Common.Core.Identity
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
            var roleClaims = _httpContext?.User.Claims.Where(x => x.Type == ClaimsIdentity.DefaultRoleClaimType);
            if (!roleClaims.Any())
            {
                return Task.FromResult(false);
            }

            var isInRole = roleClaims.Any(c => c.Value == role);

            return Task.FromResult(isInRole);
        }

        public ValueTask<string> GetClaimValue(string key)
        {
            var claim = _httpContext?.User.Claims.FirstOrDefault(x => x.Type == key);
            return ValueTask.FromResult(claim?.Value);
        }
    }
}