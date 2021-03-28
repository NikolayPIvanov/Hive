using System.Collections;
using System.Collections.Generic;
using Hive.Common.Domain;

namespace Hive.Gig.Domain.Entities
{
    public class Seller : AuditableEntity
    {
        private Seller()
        {
            Gigs = new HashSet<Gig>();
        }
        
        public int Id { get; set; }

        public string UserId { get; set; }

        public ICollection<Gig> Gigs { get; private set; }
    }
}