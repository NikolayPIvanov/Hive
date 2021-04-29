using System.Security.Claims;
using Hive.Common.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hive.Common.Core
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

    }
}