using System;
using System.Collections.Generic;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.Queries
{
    public record StateDto(OrderState OrderState, string Reason, DateTime Created);

    public record ResolutionDto(string Version, string Location, bool IsApproved);
    
    public record OrderDto(
        int Id, 
        Guid OrderNumber, 
        DateTime OrderedAt, 
        string SellerUserId, 
        string BuyerUserId,
        decimal UnitPrice,
        int Quantity,
        decimal TotalPrice,
        bool IsClosed, 
        string Requirements,
        int PackageId,
        IEnumerable<StateDto> OrderStates,
        IEnumerable<ResolutionDto> Resolution);
    
    // 
}