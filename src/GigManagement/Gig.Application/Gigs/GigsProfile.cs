using System.Linq;
using AutoMapper;
using Hive.Gig.Application.Gigs.Commands;
using Hive.Gig.Application.Gigs.Queries;

namespace Hive.Gig.Application.Gigs
{
    using Domain.Entities;
    
    public class GigsProfile : Profile
    {
        public GigsProfile()
        {
            CreateMap<Gig, GigDto>()
                .ForMember(d => d.SellerId, x => x.MapFrom(s => s.Seller.UserId))
                .ForMember(d => d.Category, x => x.MapFrom(s => s.Category.Title))
                .ForMember(d => d.Tags, x => x.MapFrom(s => s.Tags.Select(t => t.Value)))
                .ForMember(d => d.Description, x => x.MapFrom(s => s.GigScope.Description));

            /* TODO; */
            CreateMap<Gig, GigOverviewDto>()
                .ForMember(d => d.SellerUserId, x => x.MapFrom(s => s.Seller.UserId))
                .ForMember(d => d.PictureUri, x => x.Ignore())
                .AfterMap((gig, dto) =>
                {
                    if (!gig.Packages.Any())
                    {
                        dto.StartsAt = 0.0m;
                        return;
                    }

                    dto.StartsAt = gig.Packages.OrderBy(p => p.Price).First().Price;
                });
                
            
            CreateMap<Question, QuestionDto>(MemberList.Destination);
            
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