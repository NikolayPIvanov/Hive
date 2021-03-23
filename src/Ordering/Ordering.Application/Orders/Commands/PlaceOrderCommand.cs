using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Application.Publisher;
using MediatR;
using Ordering.Application.Interfaces;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.Commands
{
    public record PlaceOrderCommand : IRequest<Guid>
    {
        public string OrderedBy { get; init; } 
        public decimal UnitPrice { get; init; }
        public string Requirements { get; init; }
        
        public int SellerId { get; init; }
        public int GigId { get; init; }
        public int PackageId { get; init; }
    }
    
    // TODO: Place basic validation
    
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Guid>
    {
        private readonly IOrderingContext _context;
        private readonly IIntegrationEventPublisher _publisher;

        public PlaceOrderCommandHandler(IOrderingContext context, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }
        
        public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(request.UnitPrice, request.Requirements, request.GigId, request.PackageId,
                request.OrderedBy, request.SellerId);
            
            // TODO: send validation integration event
            var orderCreated = new OrderCreatedIntegrationEvent(order.OrderNumber, order.UnitPrice, order.OrderedBy,
                order.SellerId, order.GigId, order.PackageId);
            await _publisher.Publish(orderCreated);
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order.OrderNumber;
        }
    }
}