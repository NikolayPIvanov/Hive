namespace Hive.Gig.Domain.Entities
{
    using Hive.Common.Domain.SeedWork;

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

        public string Title { get; set; }
        
        public int? ParentId { get; set; }

        public ICollection<Category> SubCategories { get; private set; }
    }
}