using System.Collections.Generic;

namespace Hive.Gig.Domain.Entities
{
    public class Tag
    {
        private Tag()
        {
            Gigs = new HashSet<Gig>();
        }

        public Tag(string value) : this()
        {
            Value = value;
        }
        
        public int Id { get; set; }

        public string Value { get; set; }

        public ICollection<Gig> Gigs { get; private set; }
        
    }
}