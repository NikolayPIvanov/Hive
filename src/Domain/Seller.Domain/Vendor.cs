using System;
using System.Collections.Generic;

namespace Hive.Seller.Domain
{
    public class Vendor
    {
        public Guid Id { get; set; }

        public List<Plan> Plans { get; set; }
        
        public List<Gig> Gigs { get; set; }
    }
}