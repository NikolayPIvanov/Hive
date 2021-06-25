// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Collections.Generic;
using Hive.Identity.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Hive.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public sealed class ApplicationUser : IdentityUser
    {
        private ApplicationUser()
        {
        }

        public ApplicationUser(IdentityType accountType) : this()
        {
            AccountType = new UserAccountType(accountType, Id);
        }
        
        public UserAccountType AccountType { get; private set; }
        public int AccountTypeId { get; set; }

        public int? ExternalAccountId { get; set; }
    }
}