using AutoMapper;
using Hive.Gig.Application.Reviews.Queries;
using Hive.Gig.Domain.Entities;

namespace Hive.Gig.Application.Reviews
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>().DisableCtorValidation().ReverseMap();
        }
    }
}