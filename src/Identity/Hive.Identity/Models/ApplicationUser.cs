// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Collections.Generic;
using Hive.Identity.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Hive.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        private ApplicationUser()
        {
            AccountTypes = new HashSet<UserAccountType>();
        }

        public ApplicationUser(IEnumerable<IdentityType> accountTypes) : this()
        {
            foreach (var accountType in accountTypes)
            {
                AccountTypes.Add(new UserAccountType() { UserId = Id, Type = accountType});
            }
        }
        
        public ICollection<UserAccountType> AccountTypes { get; private set; }

        public int? BuyerId { get; set; }
        public int? SellerId { get; set; }
        public int? InvestorId { get; set; }
    }
}