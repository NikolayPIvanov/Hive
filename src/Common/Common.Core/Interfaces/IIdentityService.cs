using System.Threading.Tasks;
using Hive.Common.Core.Models;

namespace Hive.Common.Core.Interfaces
{
    public interface IIdentityService
    {

        Task<bool> IsInRoleAsync(string userId, string role);

        ValueTask<string?> GetClaimValue(string key);
    }
}