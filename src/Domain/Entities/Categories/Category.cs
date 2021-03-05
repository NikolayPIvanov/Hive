using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Categories
{
    public class Category : AuditableEntity
    {
        public Category()
        {
            SubCategories = new List<Category>();
        }
        public int Id { get; set; }

        public string Title { get; set; }
        
        public int? ParentCategoryId { get; set; }

        public List<Category> SubCategories { get; set; }
    }
}