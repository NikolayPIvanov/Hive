using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Orders;

namespace Hive.Domain.Entities.Gigs
{
    public class Seller : AuditableEntity
    {
        private Seller()
        {
            Gigs = new HashSet<Gig>();
            Orders = new HashSet<Order>();
        }

        public Seller(string userId) : this()
        {
            UserId = userId;
        }
        
        public string UserId { get; private init; }
        
        public ICollection<Gig> Gigs { get; private set; }
        public ICollection<Order> Orders { get; private set; }
    }
}