using AutoMapper;
using Ordering.Contracts;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .AfterMap((order, dto) =>
                {
                    dto.Status = order.Status.Status.ToString();
                    dto.Reason = order.Status.Reason;
                    dto.Requirements = order.Requirement.Details;
                });
        }
    }
}