using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Gigs;

namespace Hive.Domain.Entities.Accounts
{
    public class Seller : AuditableEntity
    {
        public Seller()
        {
            Gigs = new List<Gig>();
        }
        
        public int Id { get; set; }

        public string UserId { get; set; }

        public bool IsDraft { get; set; } = true;

        public int UserProfileId { get; set; }
        
        public UserProfile UserProfile { get; set; }
        
        public List<Gig> Gigs { get; private set; }
    }
}