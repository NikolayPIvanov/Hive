using Hive.Domain.Entities.Accounts;
using Microsoft.AspNetCore.Identity;

namespace Hive.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public AccountType AccountType { get; set; }

        public int UserProfileId { get; set; }

        public UserProfile Profile { get; set; }
    }
}
