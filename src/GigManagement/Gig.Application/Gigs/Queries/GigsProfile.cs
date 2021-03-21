using System.Linq;
using AutoMapper;
using Gig.Contracts;

namespace Hive.Gig.Application.Gigs.Queries
{
    public class GigsProfile : Profile
    {
        public GigsProfile()
        {
            CreateMap<Domain.Entities.Gig, GigDto>()
                .ForMember(d => d.Category, x => x.MapFrom(s => s.Category.Title))
                .ForMember(d => d.Tags, x => x.MapFrom(s => s.Tags.Select(t => t.Value)));
        }
    }
}