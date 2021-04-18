using AutoMapper;
using Hive.Application.Ordering.Orders.Queries;
using Hive.Application.Ordering.Resolutions.Queries;
using Hive.Domain.Entities.Orders;

namespace Hive.Application.Ordering.Resolutions
{
    public class ResolutionProfile : Profile
    {
        public ResolutionProfile()
        {
            CreateMap<Resolution, ResolutionDto>()
                .AfterMap((resolution, dto) =>
                {
                    dto.OrderNumber = resolution.Order.OrderNumber;
                });
        }
    }
}