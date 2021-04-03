namespace Hive.Gig.Domain.Entities
{
    using Hive.Common.Domain.SeedWork;
    
    using System.Collections.Generic;

    public record Tag(string Value);
    
    public class Gig : Entity
    {
        private Gig()
        {
            Tags = new HashSet<Tag>(5);
            Packages = new HashSet<Package>(3);
            Questions = new HashSet<Question>(10);
            Reviews = new HashSet<Review>(50);
        }
        
        public Gig(string title, int categoryId, ICollection<Tag> tags) : this()
        {
            Title = title;
            CategoryId = categoryId;
            Tags = tags;
        }
        
        public string Title { get; set; }

        public bool IsDraft { get; set; }

        public int? GigScopeId { get; set; }

        public GigScope GigScope { get; set; }

        public int CategoryId { get; set; }
        
        public Category Category { get; set; }

        public int SellerId { get; set; }

        public Seller Seller { get; set; }

        public ICollection<Tag> Tags { get; private set; }
        
        public ICollection<Package> Packages { get; private set; }
        
        public ICollection<Question> Questions { get; private set;  }
        
        public ICollection<Review> Reviews { get; private set;  }
    }
}