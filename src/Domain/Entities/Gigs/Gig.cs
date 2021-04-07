﻿using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Accounts;
using Hive.Domain.Entities.Investments;
using Hive.Domain.Entities.Orders;

namespace Hive.Domain.Entities.Gigs
{
    public record Tag(string Value);
    
    public class Gig : AuditableEntity
    {
        private Gig()
        {
            Tags = new HashSet<Tag>(5);
            Packages = new HashSet<Package>(3);
            Questions = new HashSet<Question>();
            Reviews = new HashSet<Review>();
        }
        
        public Gig(string title, int categoryId, int sellerId, ICollection<Tag> tags) : this()
        {
            Title = title;
            CategoryId = categoryId;
            Tags = tags;
            SellerId = sellerId;
            IsDraft = true;
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