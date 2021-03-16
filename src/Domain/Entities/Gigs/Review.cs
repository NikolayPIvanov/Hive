using Hive.Domain.Common;

namespace Hive.Domain.Entities.Gigs
{
    public class Review : AuditableEntity
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public string UserId { get; set; }

        public int GigId { get; set; }

        public double Rating { get; set; }
    }
}