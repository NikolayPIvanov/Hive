using AutoMapper;
using Hive.Application.Ordering.Orders.Queries;
using Hive.Domain.Entities.Orders;
using Hive.Domain.ValueObjects;

namespace Hive.Application.Ordering.Orders
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