using System.Linq;
using AutoMapper;
using Hive.Gig.Application.Gigs.Commands;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Contracts.Objects;

namespace Hive.Gig.Application.Gigs
{
    using Domain.Entities;
    
    public class GigsProfile : Profile
    {
        public GigsProfile()
        {
            CreateMap<Gig, GigDto>()
                .ForMember(d => d.Category, x => x.MapFrom(s => s.Category.Title))
                .ForMember(d => d.Tags, x => x.MapFrom(s => s.Tags.Select(t => t.Value)))
                .ForMember(d => d.Description, x => x.MapFrom(s => s.GigScope.Description));

            CreateMap<UpdateGigCommand, Gig>()
                .ForMember(d => d.Tags,
                    x => x.MapFrom(s =>
                        s.Tags.Select(t => new Tag(t))))
                .ForMember(d => d.Questions,
                    x => x.MapFrom(s =>
                        s.Questions.Select(t => new Question(t.Title, t.Answer))))
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