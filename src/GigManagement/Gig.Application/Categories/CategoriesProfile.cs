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
            CreateMap<Category, CategoryDto>()
                .AfterMap(
                    (category, dto) =>
                    {
                        dto.ParentOverview = category.Parent == null
                            ? null
                            : new ParentOverview()
                            {
                                Description = category.Parent.Description, Id = category.Parent.Id,
                                Title = category.Parent.Title
                            };
                    });
        }
    }
}