using AutoMapper;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Objects;

namespace Hive.Gig.Application.Categories
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}