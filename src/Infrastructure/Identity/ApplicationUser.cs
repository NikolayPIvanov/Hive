using System.Collections.Generic;
using Hive.Domain.Entities.Accounts;
using Hive.Domain.Entities.Billing;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Entities.Investing;
using Hive.Domain.Entities.Orders;
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
            AccountHolder = new AccountHolder();
        }

        public int BillingAccountId { get; set; }
        public AccountHolder AccountHolder { get; set; }
        
        public AccountType AccountType { get; set; }

        public int UserProfileId { get; set; }

        public UserProfile Profile { get; set; }

        public int? BuyerId { get; set; }

        public Buyer Buyer { get; set; }

        public int? SellerId { get; set; }
        public Seller Seller { get; set; }

        public int? InvestorId { get; set; }
        public Investor Investor { get; set; }
    }
}
