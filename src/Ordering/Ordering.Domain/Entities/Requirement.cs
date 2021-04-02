using Hive.Common.Domain;
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
        
        public int Id { get; set; }
        
        public string Details { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        
    }
}