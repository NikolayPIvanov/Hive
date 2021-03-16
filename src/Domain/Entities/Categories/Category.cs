using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Gigs;

namespace Hive.Domain.Entities.Categories
{
    public class Category : AuditableEntity
    {
        public Category()
        {
            SubCategories = new List<Category>();
            Gigs = new();
        }
        
        public int Id { get; set; }

        public string Title { get; set; }
        
        public int? ParentCategoryId { get; set; }

        public List<Category> SubCategories { get; set; }

        public List<Gig> Gigs { get; set; }
    }
}