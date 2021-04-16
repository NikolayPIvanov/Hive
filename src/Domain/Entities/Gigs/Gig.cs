using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Gigs
{
    public record Tag(string Value);
    
    public class Gig : AuditableEntity
    {
        private Gig()
        {
            Packages = new HashSet<Package>(3);
            Reviews = new HashSet<Review>();
            Questions ??= new HashSet<Question>();
            Tags ??= new HashSet<Tag>();
        }
        
        public Gig(string title, string description, int categoryId, int sellerId, ICollection<Tag> tags, ICollection<Question> questions) : this()
        {
            Title = title;
            CategoryId = categoryId;
            Tags = tags;
            Questions = questions;
            SellerId = sellerId;
            GigScope = new GigScope(description);
            IsDraft = true;
        }
        
        public string Title { get; set; }

        public bool IsDraft { get; set; }
        
        public GigScope GigScope { get; set; }

        public int CategoryId { get; set; }
        
        public Category Category { get; set; }

        public int SellerId { get; set; }

        public ICollection<Tag> Tags { get; private set; }
        
        public ICollection<Package> Packages { get; private set; }
        
        public ICollection<Question> Questions { get; private set;  }
        
        public ICollection<Review> Reviews { get; private set;  }
    }
}