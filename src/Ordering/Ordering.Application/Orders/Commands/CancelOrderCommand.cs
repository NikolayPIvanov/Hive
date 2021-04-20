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
using ValidationException = FluentValidation.ValidationException;

namespace Ordering.Application.Orders.Commands
{
    [Authorize(Roles = "Buyer, Administrator")]
    public record CancelOrderCommand(Guid OrderNumber) : IRequest;
    
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator(IOrderingContext context)
        {
            RuleFor(c => c.OrderNumber)
                .MustAsync(async (n, token) =>
                {
                    var order = await context.Orders.Include(x => x.OrderStates)
                        .FirstOrDefaultAsync(x => x.OrderNumber == n);
                    if (order != null)
                    {
                        var states = order.OrderStates.Select(os => os.OrderState);
                        var orderStates = states.ToList();
                        var dataIsValid = orderStates.Contains(OrderState.OrderDataValid);
                        var balanceIsValid = orderStates.Contains(OrderState.UserBalanceValid);

                        // must not be accepted, declined, canceled,
                        var alreadyProcessed = orderStates.Where(x =>
                                x != OrderState.UserBalanceValid && x != OrderState.OrderDataValid)
                            .All(x => x <= OrderState.Canceled);

                        return dataIsValid && balanceIsValid && alreadyProcessed;
                    }
                    
                    return false;
                });
        }
    }

    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly IOrderingContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIntegrationEventPublisher _publisher;

        public CancelOrderCommandHandler(IOrderingContext context, ICurrentUserService currentUserService, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _currentUserService = currentUserService;
            _publisher = publisher;
        }
        
        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            if (order.OrderStates.Any(s => s.OrderState == OrderState.Canceled))
            {
                return Unit.Value;
            }
            
            if (order.OrderStates.Any(s => s.OrderState == OrderState.Invalid))
            {
                var failures = new ValidationFailure[]
                {
                    new("State", "Order was invalid.")
                };
                    
                throw new ValidationException(failures);
            }

            var state = new State(OrderState.Canceled, "Order canceled by user");
            order.OrderStates.Add(state);

            var orderCanceledEvent = new OrderCanceledIntegrationEvent(order.OrderNumber, _currentUserService.UserId);
            await _publisher.Publish(orderCanceledEvent);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}