using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands
{
    public record AcceptOrderCommand(Guid OrderNumber) : IRequest;
    
    public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand>
    {
        private readonly IOrderingContext _context;

        public AcceptOrderCommandHandler(IOrderingContext context)
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

            var dataIsValid = order.OrderStates.Any(s => s.OrderState == OrderState.OrderValid);
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