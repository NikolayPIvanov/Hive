using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.Commands
{
    public record PlaceOrderCommand(decimal UnitPrice, string Requirements,
            string SellerUserId, int PackageId) : IRequest<Guid>;

    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidator()
        {
            RuleFor(x => x.Requirements)
                .NotEmpty().WithMessage("A {PropertyName} must be provided");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0.0m).WithMessage("{PropertyName} cannot be below {ComparisonValue}.")
                .NotNull().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.SellerUserId)
                .NotNull().WithMessage("A {PropertyName} must be provided");

            RuleFor(x => x.PackageId)
                .GreaterThan(0).WithMessage("{PropertyName} cannot be below {ComparisonValue}.")
                .NotNull().WithMessage("A {PropertyName} must be provided");
        }
    }
    
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Guid>
    {
        private readonly IOrderingContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIntegrationEventPublisher _publisher;

        public PlaceOrderCommandHandler(IOrderingContext context, ICurrentUserService currentUserService, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _currentUserService = currentUserService;
            _publisher = publisher;
        }
        
        public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var buyerId = await _context.Buyers.Select(b => new {b.Id, b.UserId})
                .FirstOrDefaultAsync(b => b.UserId == _currentUserService.UserId, cancellationToken: cancellationToken);

            if (buyerId == null)
            {
                throw new NotFoundException(nameof(Buyer), _currentUserService.UserId);
            }

            var order = new Order(request.UnitPrice, request.Requirements, request.PackageId, buyerId.Id, request.SellerUserId);
            
            var orderCreated = new OrderPlacedIntegrationEvent(order.OrderNumber, order.UnitPrice, buyerId.UserId,
                order.SellerUserId, order.PackageId);
            await _publisher.Publish(orderCreated);
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order.OrderNumber;
        }
    }
}