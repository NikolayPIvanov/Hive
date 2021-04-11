using System.Collections.Generic;
using Hive.Domain.Entities.Accounts;
using Hive.Domain.Entities.Gigs;
using Microsoft.AspNetCore.Identity;

namespace Hive.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        private ApplicationUser()
        {
        }

        public ApplicationUser(AccountType type) : this()
        {
            AccountType = type;
            Profile = new UserProfile();
        }
        
        public AccountType AccountType { get; set; }

        public int UserProfileId { get; set; }

        public UserProfile Profile { get; set; }

        public int? BuyerId { get; set; }

        public int? SellerId { get; set; }

        public int? InvestorId { get; set; }
    }
}
