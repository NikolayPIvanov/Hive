using AutoMapper;
using Gig.Contracts;
using Hive.Gig.Domain.Entities;

namespace Hive.Gig.Application.Categories.Queries
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}