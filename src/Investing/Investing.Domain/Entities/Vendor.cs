using System.Collections.Generic;
using Hive.Common.Core.SeedWork;

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

        public string UserId { get; private init; }

        public ICollection<Plan> Plans { get; set; }
    }
}