using Hive.Common.Domain;

namespace Hive.Gig.Domain.Entities
{
    public class GigScope : AuditableEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int GigId { get; set; }

        public Gig Gig { get; set; }
    }
}