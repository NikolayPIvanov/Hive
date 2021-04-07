using AutoMapper;
using Hive.Application.Reviews.Queries;
using Hive.Domain.Entities.Gigs;

namespace Hive.Application.Reviews
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>().ReverseMap();
        }
    }
}