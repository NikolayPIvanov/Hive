using System.Collections.Generic;
using Hive.Common.Core.SeedWork;

namespace Ordering.Domain.Entities
{
    public class Buyer : Entity
    {
        private Buyer()
        {
            Orders = new HashSet<Order>();
        }

        public Buyer(string userId) : this()
        {
            UserId = userId;
        }
        
        public string UserId { get; private init; }

        public ICollection<Order> Orders { get; private set; }
    }
}