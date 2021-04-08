using AutoMapper;
using Hive.Application.Ordering.Orders.Queries;
using Hive.Domain.Entities.Orders;

namespace Hive.Application.Ordering.Resolutions
{
    public class ResolutionProfile : Profile
    {
        public ResolutionProfile()
        {
            CreateMap<Resolution, ResolutionDto>().DisableCtorValidation();
        }
    }
}