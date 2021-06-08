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
                .ForMember(d => d.ImagePath, x
                    => x.MapFrom(s => s.Images.FirstOrDefault()))
                .ForMember(d => d.SellerId, x => x.MapFrom(s => s.SellerId))
                .ForMember(d => d.SellerUserId, x => x.MapFrom(s => s.Seller.UserId))
                .ForMember(d => d.Category, x => x.MapFrom(s => s.Category.Title))
                .ForMember(d => d.Tags, x => x.MapFrom(s => s.Tags.Select(t => t.Value)))
                .ForMember(d => d.Description, x => x.MapFrom(s => s.GigScope.Description));
    

            CreateMap<ImagePath, ImagePathDto>().DisableCtorValidation();

            CreateMap<Gig, GigOverviewDto>()
                .ForMember(d => d.SellerUserId,
                    x => x.MapFrom(s => s.Seller.UserId))
                .ForMember(d => d.ImagePath, x
                    => x.MapFrom(s => s.Images.FirstOrDefault()))
                .ForMember(d => d.Prices,
                    x => x.MapFrom(o => o.Packages.Select(p => p.Price)));
                
            
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