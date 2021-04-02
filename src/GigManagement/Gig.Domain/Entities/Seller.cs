using System.Collections.Generic;
using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Hive.Gig.Domain.Entities
{
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
        
        public int Id { get; set; }

        public string UserId { get; set; }

        public ICollection<Gig> Gigs { get; private set; }
    }
}