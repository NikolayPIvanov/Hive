using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using Hive.Domain.Enums;
using Hive.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = Hive.Application.Common.Exceptions.ValidationException;

namespace Hive.Application.Ordering.Orders.Commands
{
    public record AcceptOrderCommand(Guid OrderNumber) : IRequest;

    public class AcceptOrderCommandValidator : AbstractValidator<AcceptOrderCommand>
    {
        public AcceptOrderCommandValidator(IApplicationDbContext context)
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
    
    public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AcceptOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
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

            var seller = await _context.Sellers.FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);
            if (!dataIsValid || !balanceIsValid || order.SellerId != seller?.Id)
            {
                var failures = Array.Empty<ValidationFailure>();
                throw new ValidationException(failures);
            }
            
            var state = new State(OrderState.Accepted, "Order accepted by seller");
            order.OrderStates.Add(state);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}