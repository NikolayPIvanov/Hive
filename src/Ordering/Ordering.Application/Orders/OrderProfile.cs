using AutoMapper;
using Ordering.Contracts;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<State, StateDto>(MemberList.Destination);
        }
    }
}