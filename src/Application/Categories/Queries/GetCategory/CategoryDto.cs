using System.Collections.Generic;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities;
using Hive.Domain.Entities.Categories;

namespace Hive.Application.Categories.Queries.GetCategory
{
    public class CategoryDto : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<CategoryDto> SubCategories { get; set; }
    }
}