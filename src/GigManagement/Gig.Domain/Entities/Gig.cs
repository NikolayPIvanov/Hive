using System.Collections.Generic;
using Hive.Common.Domain;

namespace Hive.Gig.Domain.Entities
{
    public class Gig : AuditableEntity
    {
        private Gig()
        {
            Tags = new HashSet<Tag>(5);
        }
        
        public Gig(string title, ICollection<Tag> tags) : this()
        {
            Title = title;
            Tags = tags;
        }
        
        public int Id { get; set; }
        
        public string Title { get; set; }

        public bool IsDraft { get; set; } = true;

        public int GigScopeId { get; set; }

        public GigScope GigScope { get; set; }

        public int CategoryId { get; set; }
        
        public Category Category { get; set; }

        public ICollection<Tag> Tags { get; private set; }
    }
}