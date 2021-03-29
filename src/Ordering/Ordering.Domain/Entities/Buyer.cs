using System.Collections.Generic;
using Hive.Common.Domain;

namespace Ordering.Domain.Entities
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
        
        public int Id { get; set; }

        public string UserId { get; set; }

        public ICollection<Order> Orders { get; private set; }
    }
}