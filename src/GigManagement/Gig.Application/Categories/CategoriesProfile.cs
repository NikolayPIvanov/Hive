using AutoMapper;
using Hive.Gig.Contracts.Objects;
using Hive.Gig.Domain.Entities;

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