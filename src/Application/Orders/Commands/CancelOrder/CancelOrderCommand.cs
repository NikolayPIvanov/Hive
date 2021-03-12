using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Orders.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest
    {
        public Guid OrderNumber { get; set; }
    }
    
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CancelOrderCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }   
        
        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken);

            var orderIsNull = order is null;

            if (orderIsNull)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            var currentUserId = await _identityService.GetCurrentUserId();
            order.Cancel(currentUserId);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}