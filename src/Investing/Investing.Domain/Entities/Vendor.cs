using System.Collections.Generic;
using Hive.Common.Domain;

namespace Hive.Investing.Domain.Entities
{
    public class Vendor : AuditableEntity
    {
        private Vendor()
        {
            Plans = new HashSet<Plan>();
        }

        public Vendor(string userId) : this()
        {
            UserId = userId;
        }

        public int Id { get; set; }
        
        public string UserId { get; set; }

        public ICollection<Plan> Plans { get; private set; }
    }
}