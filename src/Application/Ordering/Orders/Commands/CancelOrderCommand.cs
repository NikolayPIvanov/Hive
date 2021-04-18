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
using ValidationException = FluentValidation.ValidationException;

namespace Hive.Application.Ordering.Orders.Commands
{
    public record CancelOrderCommand(Guid OrderNumber) : IRequest;

    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator(IApplicationDbContext context)
        {
            RuleFor(c => c.OrderNumber)
                .MustAsync(async (n, token) =>
                {
                    var order = await context.Orders.Include(x => x.OrderStates)
                        .FirstOrDefaultAsync(x => x.OrderNumber == n);
                    if (order != null)
                    {
                        // must be valid
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
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CancelOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
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

            var account = await _context.AccountHolders
                .Include(x => x.Wallet)
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            var transaction =
                new Transaction(order.UnitPrice, order.OrderNumber, TransactionType.Fund, account.WalletId);
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}