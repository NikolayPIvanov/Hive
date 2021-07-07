using AutoMapper;
using Ordering.Application.Orders.Queries;
using Ordering.Contracts;
using Ordering.Domain.Entities;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.Requirements, x => x.MapFrom(s => s.Requirement.Details))
                .ForMember(d => d.BuyerUserId, x => x.MapFrom(s => s.Buyer.UserId))
                .DisableCtorValidation();
            
            CreateMap<Resolution, ResolutionDto>()
                .DisableCtorValidation();
            
            CreateMap<State, StateDto>()
                .DisableCtorValidation();
        }
    }
}