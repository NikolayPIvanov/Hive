using System.Linq;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.Gig.Application.Categories.Queries;
using Hive.Gig.Domain.Entities;

namespace Hive.Gig.Application.Categories
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, ParentOverview>();

            CreateMap<Category, CategoryDto>()
                .MaxDepth(2)
                .ForMember(x => x.ParentOverview, expression => expression.MapFrom(s => s.Parent));
        }
    }
}