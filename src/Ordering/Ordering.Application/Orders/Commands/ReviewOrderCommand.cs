using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;
using ValidationException = FluentValidation.ValidationException;

namespace Ordering.Application.Orders.Commands
{
    public record ReviewOrderCommand(Guid OrderNumber, OrderState OrderState, string? Reason) : IRequest;
    
    public class ReviewOrderCommandValidator : AbstractValidator<ReviewOrderCommand>
    {
        public ReviewOrderCommandValidator(IOrderingContext context)
        {
            RuleFor(x => x.OrderNumber)
                .NotNull().WithMessage("Order Number cannot be missing")
                .MustAsync(async (orderNumber, cancellationToken) =>
                {
                    var order = await context.Orders.Include(x => x.OrderStates)
                        .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);
                    if (order == null) return false;
                    {
                        var orderStates = order.OrderStates.Select(os => os.OrderState).ToList();
                        var dataIsValid = orderStates.Contains(OrderState.OrderDataValid);
                        var balanceIsValid = orderStates.Contains(OrderState.UserBalanceValid);

                        var alreadyProcessed = orderStates.Where(x =>
                                x != OrderState.UserBalanceValid && x != OrderState.OrderDataValid)
                            .All(x => x <= OrderState.Canceled);
                        
                        return dataIsValid && balanceIsValid && alreadyProcessed;
                    }
                });
        }
    }
    
    public class ReviewOrderCommandHandler : IRequestHandler<ReviewOrderCommand>
    {
        private readonly IOrderingContext _context;
        private readonly ILogger<ReviewOrderCommandHandler> _logger;

        public ReviewOrderCommandHandler(IOrderingContext context, ILogger<ReviewOrderCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(ReviewOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken);

            if (order is null)
            {
                _logger.LogWarning("Order with number: {@Id} was not found", request.OrderNumber);
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            if (order.OrderStates.Any(s => s.OrderState == request.OrderState))
            {
                _logger.LogInformation("Order with number: {@Id} is already in {@State} state.", request.OrderNumber, request.OrderState);
                return Unit.Value;
            }

            AssertOrderIsValid(request, order);
            
            var state = new State(request.OrderState, request.Reason);
            order.OrderStates.Add(state);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        
        private void AssertOrderIsValid(ReviewOrderCommand request, Order order)
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