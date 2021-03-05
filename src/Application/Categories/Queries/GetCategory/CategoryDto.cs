using System.Collections.Generic;
using AutoMapper;
using Hive.Application.Common.Mappings;
using Hive.Application.TodoLists.Queries.GetTodos;
using Hive.Domain.Entities;
using Hive.Domain.Entities.Categories;

namespace Hive.Application.Categories.Queries.GetCategory
{
    public class CategoryDto : IMapFrom<Category>
    {
        public CategoryDto()
        {
            SubCategories = new List<CategoryDto>();
        }
        
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public int? ParentId { get; set; }

        public List<CategoryDto> SubCategories { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryDto>()
                .ForMember(d => d.ParentId, opt => opt.MapFrom(s => s.ParentCategoryId));
        }
    }
}