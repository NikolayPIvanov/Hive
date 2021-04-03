using System.Collections.Generic;
using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Hive.Investing.Domain.Entities
{
    public class Vendor : Entity
    {
        private Vendor()
        {
            Plans = new HashSet<Plan>();
        }

        public Vendor(string userId) : this()
        {
            UserId = userId;
        }
        
        public string UserId { get; set; }

        public ICollection<Plan> Plans { get; private set; }
    }
}