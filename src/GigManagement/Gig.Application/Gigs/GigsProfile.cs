using System.Linq;
using AutoMapper;
using Gig.Contracts;
using Hive.Gig.Application.Gigs.Commands;
using Hive.Gig.Domain.Entities;

namespace Hive.Gig.Application.Gigs
{
    public class GigsProfile : Profile
    {
        public GigsProfile()
        {
            CreateMap<Domain.Entities.Gig, GigDto>()
                .ForMember(d => d.Category, x => x.MapFrom(s => s.Category.Title))
                .ForMember(d => d.Tags, x => x.MapFrom(s => s.Tags.Select(t => t.Value)))
                .ForMember(d => d.Description, x => x.MapFrom(s => s.GigScope.Description));

            CreateMap<UpdateGigCommand, Domain.Entities.Gig>()
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