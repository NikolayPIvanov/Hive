using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.Commands
{
    // todo: might merge to one action
    public record DeclineOrderCommand(Guid OrderNumber) : IRequest;
    
    public class DeclineOrderCommandHandler : IRequestHandler<DeclineOrderCommand>
    {
        private readonly IOrderingContext _context;

        public DeclineOrderCommandHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeclineOrderCommand request, CancellationToken cancellationToken)
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
            
            var state = new State(OrderState.Declined, "Order declined by seller");
            order.OrderStates.Add(state);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}