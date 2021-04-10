using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using Hive.Domain.Enums;
using Hive.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Ordering.Orders.Commands
{
    public record AcceptOrderCommand(Guid OrderNumber) : IRequest;
    
    public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand>
    {
        private readonly IApplicationDbContext _context;

        public AcceptOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Select(x => new
                {
                    x.OrderNumber,
                    x.OrderStates
                })
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            if (order.OrderStates.Any(s => s.OrderState == OrderState.Accepted))
            {
                return Unit.Value;
            }

            var dataIsValid = order.OrderStates.Any(s => s.OrderState == OrderState.OrderValid);
            var balanceIsValid = order.OrderStates.Any(s => s.OrderState == OrderState.UserBalanceValid);
            
            if (!dataIsValid || !balanceIsValid)
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