using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Hive.Gig.Domain.Entities
{
    public class Review : Entity
    {
        public int Id { get; set; }

        public string Comment { get; set; }
        
        public double Rating { get; set; }

        public string UserId { get; set; }

        public int GigId { get; set; }

        public Gig Gig { get; set; }
    }
}