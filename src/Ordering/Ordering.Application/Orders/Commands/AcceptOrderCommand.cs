using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using OrderStatus = Ordering.Domain.Enums.OrderStatus;

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
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            order.Status.Status = OrderStatus.Accepted;
            order.Status.Reason = "Order accepted by seller";

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}