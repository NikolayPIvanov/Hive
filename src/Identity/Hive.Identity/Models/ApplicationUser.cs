﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Collections.Generic;
using Hive.Identity.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Hive.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public sealed class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }
        
        public int? ExternalAccountId { get; set; }
    }
}