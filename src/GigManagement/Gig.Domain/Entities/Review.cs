namespace Hive.Gig.Domain.Entities
{
    using Hive.Common.Domain.SeedWork;

    public class Review : Entity
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