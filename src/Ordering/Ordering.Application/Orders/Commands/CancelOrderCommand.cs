using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Application.Orders.EventHandlers;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;
using ValidationException = FluentValidation.ValidationException;

namespace Ordering.Application.Orders.Commands
{

    public record CancelOrderCommand(Guid OrderNumber) : IRequest;
    
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator(IOrderingContext context)
        {
            RuleFor(c => c.OrderNumber)
                .MustAsync(async (orderNumber, token) =>
                {
                    var order = await context.Orders
                        .Include(x => x.OrderStates)
                        .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber, token);

                    if (order == null) return false;
                    {
                        var orderStates = order.OrderStates.Select(os => os.OrderState).ToList();
                        var dataIsValid = orderStates.Contains(OrderState.OrderDataValid);
                        var balanceIsValid = orderStates.Contains(OrderState.UserBalanceValid);

                        // must not be accepted, declined, canceled
                        var alreadyProcessed = orderStates.Where(x =>
                                x != OrderState.UserBalanceValid && x != OrderState.OrderDataValid)
                            .All(x => x <= OrderState.Canceled);

                        return dataIsValid && balanceIsValid && alreadyProcessed;
                    }
                });
        }
    }

    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly IOrderingContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CancelOrderCommandHandler> _logger;

        public CancelOrderCommandHandler(IOrderingContext context, ICurrentUserService currentUserService, ILogger<CancelOrderCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken);

            if (order is null)
            {
                _logger.LogWarning("Order with number: {@Id} was not found", request.OrderNumber);
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            if (order.OrderStates.Any(s => s.OrderState == OrderState.Canceled))
            {
                _logger.LogInformation("Order with number: {@Id} is already in canceled state.", request.OrderNumber);
                return Unit.Value;
            }
            
            AssertOrderIsValid(request, order);

            var state = new State(OrderState.Canceled, "Order canceled by user");
            order.OrderStates.Add(state);
            order.AddDomainEvent(new OrderCanceledEvent(order.OrderNumber, _currentUserService.UserId));

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }

        private void AssertOrderIsValid(CancelOrderCommand request, Order order)
        {
            if (order.OrderStates.All(s => s.OrderState != OrderState.Invalid)) return;
            var failures = new ValidationFailure[]
            {
                new("State", "Order is invalid.")
            };

            _logger.LogWarning("Order with number: {@Id} was is not valid.", request.OrderNumber);
            throw new ValidationException(failures);
        }
    }
}