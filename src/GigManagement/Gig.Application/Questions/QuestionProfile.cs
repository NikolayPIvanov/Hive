using AutoMapper;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Objects;

namespace Hive.Gig.Application.Questions
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDto>();
        }
    }
}