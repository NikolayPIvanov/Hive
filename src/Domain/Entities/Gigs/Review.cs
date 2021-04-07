using Hive.Domain.Common;

namespace Hive.Domain.Entities.Gigs
{
    public class Review : AuditableEntity
    {
        private Review()
        {
        }

        public Review(string userId, int gigId, string comment, double rating) : this()
        {
            UserId = userId;
            GigId = gigId;
            Comment = comment;
            Rating = rating;
        }
        
        public string Comment { get; set; }
        
        public double Rating { get; set; }

        public string UserId { get; private init; }

        public int GigId { get; set; }
    }
}