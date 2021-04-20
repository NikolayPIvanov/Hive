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
    public record AcceptOrderCommand(Guid OrderNumber) : IRequest;
    
    public class AcceptOrderCommandValidator : AbstractValidator<AcceptOrderCommand>
    {
        public AcceptOrderCommandValidator(IOrderingContext context)
        {
            RuleFor(x => x.OrderNumber)
                .NotNull().WithMessage("Order Number cannot be missing")
                .MustAsync(async (orderNumber, cancellationToken) =>
                {
                    var order = await context.Orders.Include(x => x.OrderStates)
                        .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);
                    if (order == null) return false;
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
                });
        }
    }
    
    public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand>
    {
        private readonly IOrderingContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AcceptOrderCommandHandler(IOrderingContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<Unit> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            if (order.OrderStates.Any(s => s.OrderState == OrderState.Accepted))
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
            
            var state = new State(OrderState.Accepted, "Order accepted by seller");
            order.OrderStates.Add(state);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}