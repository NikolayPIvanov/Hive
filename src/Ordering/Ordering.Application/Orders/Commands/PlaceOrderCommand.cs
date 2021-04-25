using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Application.Orders.EventHandlers;
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
                .MinimumLength(10).WithMessage("{PropertyName} should be more than {MinimumLength} characters long.")
                .MaximumLength(3000).WithMessage("{PropertyName} should be more than {MaximumLength} characters long.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0.0m).WithMessage("{PropertyName} cannot be below {ComparisonValue}.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.SellerUserId)
                .Must(id => id != Guid.Empty.ToString()).WithMessage("{Property} cannot accept default Guid value.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");

            RuleFor(x => x.PackageId)
                .GreaterThan(0).WithMessage("{PropertyName} cannot be below {ComparisonValue}.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
        }
    }
    
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Guid>
    {
        private readonly IOrderingContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIntegrationEventPublisher _publisher;
        private readonly ILogger<PlaceOrderCommandHandler> _logger;

        public PlaceOrderCommandHandler(IOrderingContext context, ICurrentUserService currentUserService,
            ILogger<PlaceOrderCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var buyerId = await _context.Buyers.Select(b => new {b.Id, b.UserId})
                .FirstOrDefaultAsync(b => b.UserId == _currentUserService.UserId, cancellationToken: cancellationToken);

            if (buyerId == null)
            {
                _logger.LogWarning("Buyer for user with id: {@Id} was not found", _currentUserService.UserId);
                throw new NotFoundException(nameof(Buyer), _currentUserService.UserId);
            }

            var order = new Order(request.UnitPrice, request.Requirements, request.PackageId, buyerId.Id, request.SellerUserId);
            order.AddDomainEvent(new OrderPlacedEvent(order, buyerId.UserId));

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order.OrderNumber;
        }
    }
}