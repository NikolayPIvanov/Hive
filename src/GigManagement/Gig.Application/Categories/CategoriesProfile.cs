using System.Collections.Generic;
using AutoMapper;
using Hive.Gig.Domain.Entities;

namespace Hive.Gig.Application.Categories
{
    public record CategoryDto(int Id, string Title, int? ParentId, IEnumerable<CategoryDto> SubCategories);

    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}