using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Gigs
{
    public class Category : AuditableEntity
    {
        private Category()
        {
            SubCategories = new HashSet<Category>();
        }

        public Category(string title, int? parentId) : this()
        {
            Title = title;
            ParentId = parentId;
        }

        public string Title { get; set; }
        
        public int? ParentId { get; set; }

        public ICollection<Category> SubCategories { get; private set; }
    }
}