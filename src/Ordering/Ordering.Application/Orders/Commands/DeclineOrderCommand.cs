using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;
using ValidationException = Hive.Common.Core.Exceptions.ValidationException;

namespace Ordering.Application.Orders.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
    public record DeclineOrderCommand(Guid OrderNumber) : IRequest;
    public class DeclineOrderCommandValidator : AbstractValidator<DeclineOrderCommand>
    {
        public DeclineOrderCommandValidator(IOrderingContext context)
        {
            RuleFor(x => x.OrderNumber)
                .NotNull().WithMessage("Cannot be null")
                .MustAsync(async (orderNumber, cancellationToken) =>
                {
                    var order = await context.Orders.Include(x => x.OrderStates)
                        .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);
                    if (order != null)
                    {
                        // must be valid
                        var states = order.OrderStates.Select(os => os.OrderState);
                        var orderStates = states.ToList();
                        var dataIsValid = orderStates.Contains(OrderState.OrderDataValid);
                        var balanceIsValid = orderStates.Contains(OrderState.UserBalanceValid);

                        // must not be accepted, declined, canceled, etc
                        var alreadyProcessed = orderStates.Where(x =>
                                x != OrderState.UserBalanceValid && x != OrderState.OrderDataValid)
                            .All(x => x <= OrderState.Canceled);
                        
                        return dataIsValid && balanceIsValid && alreadyProcessed;
                    }
                    
                    return false;
                });
        }
    }
    
    
    public class DeclineOrderCommandHandler : IRequestHandler<DeclineOrderCommand>
    {
        private readonly IOrderingContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIntegrationEventPublisher _publisher;

        public DeclineOrderCommandHandler(IOrderingContext context, ICurrentUserService currentUserService, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _currentUserService = currentUserService;
            _publisher = publisher;
        }
        
        public async Task<Unit> Handle(DeclineOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            if (order.OrderStates.Any(s => s.OrderState == OrderState.Declined))
            {
                return Unit.Value;
            }

            var dataIsValid = order.OrderStates.Any(s => s.OrderState == OrderState.OrderDataValid);
            var balanceIsValid = order.OrderStates.Any(s => s.OrderState == OrderState.UserBalanceValid);
            
            if (!dataIsValid || !balanceIsValid)
            {
                var failures = new ValidationFailure[] { };
                throw new ValidationException(failures);
            }
            
            var state = new State(OrderState.Declined, "Order declined by seller");
            order.OrderStates.Add(state);

            var orderDeclinedEvent = new OrderDeclinedIntegrationEvent(order.OrderNumber, _currentUserService.UserId);
            await _publisher.Publish(orderDeclinedEvent);
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}