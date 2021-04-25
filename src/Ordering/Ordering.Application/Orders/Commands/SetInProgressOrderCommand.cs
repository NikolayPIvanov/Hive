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
using ValidationException = Hive.Common.Core.Exceptions.ValidationException;

namespace Ordering.Application.Orders.Commands
{
    public record SetInProgressOrderCommand(Guid OrderNumber) : IRequest;
    
    public class SetInProgressOrderCommandValidator : AbstractValidator<SetInProgressOrderCommand>
    {
        public SetInProgressOrderCommandValidator(IOrderingContext context)
        {
            RuleFor(x => x.OrderNumber)
                .NotNull().WithMessage("Cannot be null")
                .MustAsync(async (orderNumber, cancellationToken) =>
                {
                    var order = await context.Orders.Include(x => x.OrderStates)
                        .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);
                    if (order == null) return false;
                    {
                        var states = order.OrderStates.Select(os => os.OrderState);
                        var orderStates = states.ToList();
                        var orderIsAccepted = orderStates.Contains(OrderState.Accepted);
                        var orderIsInProgressOrCompleted = orderStates.Any(x => x >= OrderState.InProgress);

                        return orderIsAccepted && !orderIsInProgressOrCompleted;
                    }

                });
        }
    }
    
    public class SetInProgressOrderCommandHandler : IRequestHandler<SetInProgressOrderCommand>
    {
        private readonly IOrderingContext _context;
        private readonly ILogger<SetInProgressOrderCommandHandler> _logger;

        public SetInProgressOrderCommandHandler(IOrderingContext context, ILogger<SetInProgressOrderCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(SetInProgressOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                _logger.LogWarning("Order with number: {@Id} was not found", request.OrderNumber);
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            if (order.OrderStates.Any(s => s.OrderState == OrderState.InProgress))
            {
                _logger.LogInformation("Order with number: {@Id} is already in {@State} state.", request.OrderNumber, OrderState.InProgress);
                return Unit.Value;
            }

            var orderIsAccepted = order.OrderStates.Any(s => s.OrderState == OrderState.Accepted);
            
            if (!orderIsAccepted)
            {
                var failures = new[]
                {
                    new ValidationFailure("OrderState", "Order is not accepted.")
                };
                throw new ValidationException(failures);
            }

            var state = new State(OrderState.InProgress, "Order marked in progress");
            order.OrderStates.Add(state);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}