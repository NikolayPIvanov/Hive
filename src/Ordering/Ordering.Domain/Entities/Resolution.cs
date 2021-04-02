using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Ordering.Domain.Entities
{
    public class Resolution : Entity
    {
        public int Id { get; set; }

        public string Version { get; set; }

        public string Location { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}