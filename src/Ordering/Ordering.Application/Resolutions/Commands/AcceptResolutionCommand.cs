using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Resolutions.Commands
{
    public record AcceptResolutionCommand(int ResolutionId) : IRequest;

    public class AcceptResolutionCommandHandler : IRequestHandler<AcceptResolutionCommand>
    {
        private readonly IOrderingContext _context;
        private readonly ILogger<AcceptResolutionCommandHandler> _logger;

        public AcceptResolutionCommandHandler(IOrderingContext context, ILogger<AcceptResolutionCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(AcceptResolutionCommand request, CancellationToken cancellationToken)
        {
            var resolution = await _context.Resolutions
                .Include(r => r.Order)
                .ThenInclude(o => o.OrderStates)
                .FirstOrDefaultAsync(x => x.Id == request.ResolutionId, cancellationToken: cancellationToken);
            
            if (resolution?.Order == null)
            {
                _logger.LogWarning("Order for resolution with id: {@Id} was not found", request.ResolutionId);
                throw new NotFoundException(nameof(Resolution));
            }
            
            if (resolution == null)
            {
                _logger.LogWarning("Resolution with id: {@Id} was not found", request.ResolutionId);
                throw new NotFoundException(nameof(Resolution), request.ResolutionId);
            }
            
            if (resolution.Order.OrderStates.All(x => x.OrderState != OrderState.InProgress))
            {
                throw new ValidationException(new []
                {
                    new ValidationFailure("OrderStates", "Order is not marked in progress")
                });
            }

            resolution.Order.OrderStates.Add(new State(OrderState.Completed, $"Buyer accepted resolution - {request.ResolutionId}"));
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}