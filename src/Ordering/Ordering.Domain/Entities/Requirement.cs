using Hive.Common.Core.SeedWork;
using Hive.Common.Domain.SeedWork;

namespace Ordering.Domain.Entities
{
    public class Requirement : Entity
    {
        private Requirement()
        {
        }

        public Requirement(string details) : this()
        {
            Details = details;
        }
                
        public string Details { get; set; }

        public int OrderId { get; set; }
    }
}