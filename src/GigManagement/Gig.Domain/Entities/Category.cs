using Hive.Common.Core.SeedWork;

namespace Hive.Gig.Domain.Entities
{
    using System.Collections.Generic;

    public class Category : Entity
    {
        private Category()
        {
            SubCategories = new HashSet<Category>();
        }

        public Category(string title, string description, int? parentId) : this()
        {
            Title = title;
            Description = description;
            ParentId = parentId;
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageLocation { get; set; }
        
        public int? ParentId { get; set; }

        public Category Parent { get; set; }

        public ICollection<Category> SubCategories { get; private set; }
    }
}