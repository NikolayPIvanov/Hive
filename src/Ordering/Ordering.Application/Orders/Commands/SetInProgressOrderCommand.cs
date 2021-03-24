using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.Commands
{
    public record SetInProgressOrderCommand(Guid OrderNumber) : IRequest;
    
    public class SetInProgressOrderCommandHandler : IRequestHandler<SetInProgressOrderCommand>
    {
        private readonly IOrderingContext _context;

        public SetInProgressOrderCommandHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(SetInProgressOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            order.Status.Status = Domain.Enums.OrderStatus.InProgress;
            order.Status.Reason = "Order marked in progress";

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}