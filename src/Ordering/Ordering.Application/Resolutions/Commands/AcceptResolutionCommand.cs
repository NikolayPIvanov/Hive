using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Billing.Contracts.IntegrationEvents;
using Hive.Common.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

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
                .Include(o => o.Order)
                .ThenInclude(o => o.OrderStates)
                .FirstOrDefaultAsync(x => x.Id == request.ResolutionId, cancellationToken: cancellationToken);

            if (resolution == null)
            {
                throw new NotFoundException(nameof(Resolution), request.ResolutionId);
            }

            if (resolution.Order.OrderStates.All(x => x.OrderState != OrderState.InProgress))
            {
                throw new Exception();
            }

            resolution.Order.OrderStates.Add(new State(OrderState.Completed, $"Buyer accepted resolution - {request.ResolutionId}"));
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}