using AutoMapper;
using Gig.Contracts;
using Hive.Gig.Domain.Entities;

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