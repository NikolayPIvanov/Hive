using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Orders
{
    public class Buyer : AuditableEntity
    {
        private Buyer()
        {
            Orders = new HashSet<Order>();
        }

        public Buyer(string userId) : this()
        {
            UserId = userId;
        }
        
        public string UserId { get; private set; }

        public ICollection<Order> Orders { get; private set; }
    }
}