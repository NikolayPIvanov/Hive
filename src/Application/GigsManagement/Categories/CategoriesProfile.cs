using AutoMapper;
using Hive.Application.GigsManagement.Categories.Queries.GetCategory;
using Hive.Domain.Entities.Gigs;

namespace Hive.Application.GigsManagement.Categories
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}