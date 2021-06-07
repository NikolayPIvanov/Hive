using Hive.Common.Core.SeedWork;

namespace Hive.Gig.Domain.Entities
{
    using System.Collections.Generic;

    public record Tag(string Value);
    
    public class Gig : Entity
    {
        private Gig()
        {
            Tags = new HashSet<Tag>(5);
            Packages = new HashSet<Package>(3);
            Questions = new HashSet<Question>();
            Reviews = new HashSet<Review>();
            Images = new HashSet<ImagePath>();
            IsDraft = true;
        }
        
        public Gig(string title, string description, int sellerId, int categoryId, 
            ICollection<Tag> tags,  ICollection<Question> questions, int? planId = null) : this()
        {
            Title = title;
            GigScope = new GigScope(description);
            CategoryId = categoryId;
            PlanId = planId;
            SellerId = sellerId;
            Questions = questions;
            Tags = tags;
        }
        
        public string Title { get; set; }
        public GigScope GigScope { get; set; }

        public bool IsDraft { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int SellerId { get; set; }
        public Seller Seller { get; set; }

        public int? PlanId { get; set; }

        public ICollection<Tag> Tags { get; private set; }

        public ICollection<Package> Packages { get; private set; }
        
        public ICollection<Question> Questions { get; private set;  }
        
        public ICollection<Review> Reviews { get; private set;  }
        
        public ICollection<ImagePath> Images { get; private set; }
    }
}