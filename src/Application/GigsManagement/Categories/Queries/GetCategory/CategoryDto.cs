using System.Collections.Generic;
using AutoMapper;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Gigs;

namespace Hive.Application.Categories.Queries.GetCategory
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ParentId { get; set; }

        public IEnumerable<CategoryDto> SubCategories { get; set; }
    }
}