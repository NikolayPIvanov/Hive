using AutoMapper;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Gigs;

namespace Hive.Application.Gigs.Commands.CreateGig
{
    public class QuestionDto : IMapFrom<GigQuestion>
    {
        public string Question { get; set; }
        
        public string Answer { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GigQuestion, QuestionDto>().ReverseMap();
        }
    }
}