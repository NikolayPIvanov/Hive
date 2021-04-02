using Hive.Common.Domain.SeedWork;

namespace Hive.Gig.Domain.Entities
{
    using Hive.Common.Domain;
    using System.Collections.Generic;

    public class Category : Entity
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