using System.Collections.Generic;
using Hive.Common.Domain;

namespace Hive.Gig.Domain.Entities
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
        
        public int Id { get; set; }

        public string Title { get; set; }
        
        public int? ParentId { get; set; }

        public ICollection<Category> SubCategories { get; private set; }
    }
}