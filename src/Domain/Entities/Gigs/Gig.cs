using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Accounts;
using Hive.Domain.Entities.Categories;

namespace Hive.Domain.Entities.Gigs
{
    public class Gig : AuditableEntity
    {
        public Gig()
        {
            Questions = new();
            Packages = new();
        }

        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string? Metadata { get; set; }
        
        public string Tags { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
        
        public List<Package> Packages { get; set; }
        
        public List<GigQuestion> Questions { get; set; }
        
        public Seller Seller { get; set; }

        public int SellerId { get; set; }
    }
}