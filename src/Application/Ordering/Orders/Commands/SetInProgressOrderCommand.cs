using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Entities.Orders;
using Hive.Domain.Enums;
using Hive.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = Hive.Application.Common.Exceptions.ValidationException;

namespace Hive.Application.Ordering.Orders.Commands
{
    public record SetInProgressOrderCommand(Guid OrderNumber) : IRequest;

    public class SetInProgressOrderCommandValidator : AbstractValidator<SetInProgressOrderCommand>
    {
        public SetInProgressOrderCommandValidator(IApplicationDbContext context)
        {
            RuleFor(x => x.OrderNumber)
                .NotNull().WithMessage("Cannot be null")
                .MustAsync(async (orderNumber, cancellationToken) =>
                {
                    var order = await context.Orders.Include(x => x.OrderStates)
                        .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);
                    if (order != null)
                    {
                        // must be accepted
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
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public SetInProgressOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<Unit> Handle(SetInProgressOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Seller)
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }

            if (order.Seller.UserId != _currentUserService.UserId)
            {
                throw new NotFoundException(nameof(Seller));
            }
            
            if (order.OrderStates.Any(s => s.OrderState == OrderState.InProgress))
            {
                return Unit.Value;
            }

            var state = new State(OrderState.InProgress, "Order marked in progress");
            order.OrderStates.Add(state);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}