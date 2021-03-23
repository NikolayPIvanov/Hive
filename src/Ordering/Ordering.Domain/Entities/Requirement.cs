using Hive.Common.Domain;

namespace Ordering.Domain.Entities
{
    public class Requirement : AuditableEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public string Details { get; set; }
    }
}