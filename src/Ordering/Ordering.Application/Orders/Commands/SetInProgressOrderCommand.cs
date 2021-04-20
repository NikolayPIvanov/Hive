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
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;
using ValidationException = Hive.Common.Core.Exceptions.ValidationException;

namespace Ordering.Application.Orders.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
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
                        .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);
                    if (order != null)
                    {
                        var states = order.OrderStates.Select(os => os.OrderState);
                        var orderStates = states.ToList();
                        var orderIsAccepted = orderStates.Contains(OrderState.Accepted);
                        var orderIsInProgressOrCompleted = orderStates.Any(x => x >= OrderState.InProgress);

                        return orderIsAccepted && !orderIsInProgressOrCompleted;
                    }
                    
                    return false;
                });
        }
    }
    
    public class SetInProgressOrderCommandHandler : IRequestHandler<SetInProgressOrderCommand>
    {
        private readonly IOrderingContext _context;
        private readonly ICurrentUserService _currentUserService;

        public SetInProgressOrderCommandHandler(IOrderingContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<Unit> Handle(SetInProgressOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            if (order.OrderStates.Any(s => s.OrderState == OrderState.InProgress))
            {
                return Unit.Value;
            }

            var orderIsAccepted = order.OrderStates.Any(s => s.OrderState == OrderState.Accepted);
            
            if (!orderIsAccepted)
            {
                var failures = new ValidationFailure[] { };
                throw new ValidationException(failures);
            }

            var state = new State(OrderState.InProgress, "Order marked in progress");
            order.OrderStates.Add(state);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}