using AutoMapper;
using Ordering.Contracts;
using Ordering.Domain.Entities;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>().DisableCtorValidation();
            CreateMap<State, StateDto>(MemberList.Destination).DisableCtorValidation();
        }
    }
}