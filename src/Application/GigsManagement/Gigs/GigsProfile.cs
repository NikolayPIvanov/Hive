using System.Linq;
using AutoMapper;
using Hive.Application.Gigs.Commands.UpdateGig;
using Hive.Application.Gigs.Queries;
using Hive.Domain.Entities.Gigs;

namespace Hive.Application.Gigs
{
    public class GigsProfile : Profile
    {
        public GigsProfile()
        {
            CreateMap<GigScope, GigScopeDto>();
            
            CreateMap<Gig, GigDto>()
                .ForMember(d => d.Category, x => x.MapFrom(s => s.Category.Title))
                .ForMember(d => d.Tags, x => x.MapFrom(s => s.Tags.Select(t => t.Value)))
                .ForMember(d => d.Description, x => x.MapFrom(s => s.GigScope.Description));

            CreateMap<UpdateGigCommand, Gig>()
                .ForMember(d => d.GigScopeId, x => x.Ignore())
                .ForMember(d => d.Tags,
                    x => x.MapFrom(s =>
                        s.Tags.Select(t => new Tag(t))))
                .AfterMap((command, gig) =>
                {
                    if (gig.GigScope != null)
                    {
                        gig.GigScope.Description = command.Description;
                    }
                });
        }
    }
}