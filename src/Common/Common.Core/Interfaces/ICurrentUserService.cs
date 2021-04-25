using System.Security.Claims;

namespace Hive.Common.Core.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        
        ClaimsPrincipal User { get; }
    }
}