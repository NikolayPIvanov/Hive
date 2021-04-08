using AutoMapper;
using Hive.Application.GigsManagement.Reviews.Queries;
using Hive.Domain.Entities.Gigs;

namespace Hive.Application.GigsManagement.Reviews
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>().ReverseMap();
        }
    }
}