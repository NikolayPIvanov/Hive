using System;
using System.Collections.Generic;

namespace Hive.Seller.Domain
{
    // Seller account is created upon registration as seller
    public class Seller
    {
        public Seller(Guid id)
        {
            Id = id;
            Gigs = new List<Gig>();
        }
        
        // Should be the same as user id in user management domain
        public Guid Id { get; set; }

        public List<Gig> Gigs { get; set; }
    }
}