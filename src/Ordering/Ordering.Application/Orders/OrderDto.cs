using System;
using System.Collections.Generic;

namespace Ordering.Application.Orders
{
    public record StateDto(string OrderState, string? Reason, DateTime Created, string CreatedBy);

    public record OrderDto(int Id, Guid OrderNumber, DateTime OrderedAt, string SellerUserId, string BuyerUserId,
        decimal UnitPrice,
        bool IsClosed, string Requirements, int GigId, int PackageId, ICollection<StateDto> OrderStates);
}