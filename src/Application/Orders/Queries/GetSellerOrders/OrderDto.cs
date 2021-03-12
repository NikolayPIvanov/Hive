using System;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Orders;
using Hive.Domain.Enums;

namespace Hive.Application.Orders.Queries.GetSellerOrders
{
    public class OrderDto : IMapFrom<Order>
    {
        public Guid OrderNumber { get; set; }
        
        public DateTime OrderedAt { get; private init; }
        
        public OrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }
        
        public int GigId { get; set; }
        
        public int PackageId { get; set; }
    }
}