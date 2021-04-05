using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Application.Resolutions.DomainEvents;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Resolutions.Commands
{
    public record AcceptResolutionCommand(int ResolutionId) : IRequest;

    public class AcceptResolutionCommandHandler : IRequestHandler<AcceptResolutionCommand>
    {
        private readonly IOrderingContext _context;

        public AcceptResolutionCommandHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(AcceptResolutionCommand request, CancellationToken cancellationToken)
        {
            var resolution = await _context.Resolutions
                .FirstOrDefaultAsync(x => x.Id == request.ResolutionId, cancellationToken: cancellationToken);

            if (resolution == null)
            {
                throw new NotFoundException(nameof(Resolution), request.ResolutionId);
            }

            var order = await _context.Orders
                .Include(b => b.Buyer)
                .FirstOrDefaultAsync(x => x.Id == resolution.OrderId, cancellationToken: cancellationToken);
            
            // todo: fix
            if (order.OrderStates.All(x => x.OrderState == OrderState.InProgress))
            {
                throw new Exception();
            }
            
            order.AddDomainEvent(new ResolutionAcceptedEvent(resolution.Id, order.OrderNumber, order.SellerId, order.Buyer.UserId));
            order.OrderStates.Add(new State(OrderState.Completed, $"Buyer accepted resolution - {request.ResolutionId}"));
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}