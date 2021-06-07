using System.Collections.Generic;

namespace Hive.Gig.Application.Categories.Queries
{
    public class ParentOverview
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    
    public class CategoryDto
    {
        private CategoryDto()
        {
            SubCategories = new HashSet<CategoryDto>();
        }
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ParentOverview? ParentOverview { get; set; }
        public ICollection<CategoryDto> SubCategories { get; private set; }
    }
}