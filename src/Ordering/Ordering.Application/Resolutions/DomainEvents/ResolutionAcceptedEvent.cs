using System;
using MediatR;

namespace Ordering.Application.Resolutions.DomainEvents
{
    public record ResolutionAcceptedEvent(int ResolutionId, Guid OrderNumber,
        string SellerId, string BuyerId) : INotification;
}