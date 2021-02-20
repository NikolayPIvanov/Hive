using Hive.Domain.Common;

namespace Hive.Seller.Domain
{
    public class GigQuestion : AuditableEntity
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public int GigId { get; set; }
    }
}