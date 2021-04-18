using AutoMapper;
using Hive.Application.Ordering.Orders.Queries;
using Hive.Application.Ordering.Resolutions.Queries;
using Hive.Domain.Entities.Orders;
using Hive.Domain.ValueObjects;

namespace Hive.Application.Ordering.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>().DisableCtorValidation().ReverseMap();
            CreateMap<Resolution, ResolutionDto>().DisableCtorValidation()
                .AfterMap((resolution, dto) => dto.OrderNumber = resolution.Order.OrderNumber);
            CreateMap<Requirement, RequirementDto>().DisableCtorValidation().ReverseMap();
            CreateMap<State, StateDto>().DisableCtorValidation()
                .ForMember(d => d.OrderState, x => x.MapFrom(s => s.OrderState.ToString()))
                .ReverseMap();
        }
    }
}