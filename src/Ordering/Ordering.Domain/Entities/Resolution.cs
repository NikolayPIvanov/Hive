using Hive.Common.Domain;

namespace Ordering.Domain.Entities
{
    public class Resolution : AuditableEntity
    {
        public int Id { get; set; }

        public string Version { get; set; }

        public string Location { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}