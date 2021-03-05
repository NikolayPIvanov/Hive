using Hive.Application.Common.Mappings;
using Hive.Domain.Entities;

namespace Hive.Application.Categories.Queries.GetCategory
{
    public class CategoryDto : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}