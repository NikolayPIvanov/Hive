using Hive.Domain.Common;

namespace Hive.Seller.Domain
{
    public class Package : AuditableEntity
    {
        public int Id { get; set; }

        public int GigId { get; set; }

        public Gig Gig { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        // TODO: Needs to be Enum
        public string PackageTier { get; set; }
    }
}