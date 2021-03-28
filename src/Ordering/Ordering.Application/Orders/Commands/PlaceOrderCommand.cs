using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Application.Interfaces;
using MediatR;
using Ordering.Application.Interfaces;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.Commands
{
    public record PlaceOrderCommand(string OrderedBy, decimal UnitPrice, string Requirements,
            int SellerId, int GigId, int PackageId) : IRequest<Guid>;

    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidator()
        {
            RuleFor(x => x.OrderedBy)
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.Requirements)
                .NotEmpty().WithMessage("A {PropertyName} must be provided");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0.0m).WithMessage("{PropertyName} cannot be below {ComparisonValue}.")
                .NotNull().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.SellerId)
                .GreaterThan(0).WithMessage("{PropertyName} cannot be below {ComparisonValue}.")
                .NotNull().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.GigId)
                .GreaterThan(0).WithMessage("{PropertyName} cannot be below {ComparisonValue}.")
                .NotNull().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.PackageId)
                .GreaterThan(0).WithMessage("{PropertyName} cannot be below {ComparisonValue}.")
                .NotNull().WithMessage("A {PropertyName} must be provided");
        }
    }
    
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
            
            var orderCreated = new OrderCreatedIntegrationEvent(order.OrderNumber, order.UnitPrice, order.OrderedBy,
                order.SellerId, order.GigId, order.PackageId);
            await _publisher.Publish(orderCreated);
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order.OrderNumber;
        }
    }
}