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
    public record SetInProgressOrderCommand(Guid OrderNumber) : IRequest;
    
    public class SetInProgressOrderCommandHandler : IRequestHandler<SetInProgressOrderCommand>
    {
        private readonly IApplicationDbContext _context;

        public SetInProgressOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(SetInProgressOrderCommand request, CancellationToken cancellationToken)
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