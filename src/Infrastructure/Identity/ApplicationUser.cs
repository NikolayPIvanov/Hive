using Microsoft.AspNetCore.Identity;

namespace Hive.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsSeller { get; set; }
    }
}
