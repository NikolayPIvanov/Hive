namespace Hive.Gig.Domain.Entities
{
    using System.Collections.Generic;
    using Hive.Common.Domain.SeedWork;
    
    public class Seller : Entity
    {
        
        private Seller()
        {
            Gigs = new HashSet<Gig>();
        }

        public Seller(string userId) : this()
        {
            UserId = userId;
        }
        
        public string UserId { get; private init; }

        public ICollection<Gig> Gigs { get; private set; }
    }
}