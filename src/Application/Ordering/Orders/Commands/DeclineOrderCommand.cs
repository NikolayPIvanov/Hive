using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Billing;
using Hive.Domain.Entities.Orders;
using Hive.Domain.Enums;
using Hive.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = Hive.Application.Common.Exceptions.ValidationException;

namespace Hive.Application.Ordering.Orders.Commands
{
    public record DeclineOrderCommand(Guid OrderNumber) : IRequest;

    public class DeclineOrderCommandValidator : AbstractValidator<DeclineOrderCommand>
    {
        public DeclineOrderCommandValidator(IApplicationDbContext context)
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
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeclineOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
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
            var balanceIsValid = order.OrderStates.Any(s => s.OrderState == OrderState.OrderDataValid);
            var seller = await _context.Sellers.FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            if (!dataIsValid || !balanceIsValid || order.SellerId != seller?.Id)
            {
                var failures = new ValidationFailure[] { };
                throw new ValidationException(failures);
            }

            var state = new State(OrderState.Declined, "Order declined by seller");
            order.OrderStates.Add(state);
            
            var transaction =
                new Transaction(order.UnitPrice, order.OrderNumber, TransactionType.Fund, order.BuyerId);
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}