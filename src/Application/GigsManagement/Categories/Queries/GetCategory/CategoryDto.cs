using System.Collections.Generic;

namespace Hive.Application.GigsManagement.Categories.Queries.GetCategory
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public IEnumerable<CategoryDto> SubCategories { get; set; }
    }
}