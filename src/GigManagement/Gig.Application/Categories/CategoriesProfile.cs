using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.Gig.Domain.Entities;

namespace Hive.Gig.Application.Categories
{
    public class CategoryDto
    {
        private CategoryDto()
        {
            SubCategories = new HashSet<CategoryDto>();
        }
        
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public ICollection<CategoryDto> SubCategories { get; private set; }
    }

    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}