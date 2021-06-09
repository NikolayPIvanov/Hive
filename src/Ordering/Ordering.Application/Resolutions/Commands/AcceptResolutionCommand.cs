using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using FluentValidation.Results;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Resolutions.Commands
{
    public record AcceptResolutionCommand(Guid Version) : IRequest;

    public class AcceptResolutionCommandHandler : IRequestHandler<AcceptResolutionCommand>
    {
        private readonly IOrderingContext _context;
        private readonly IIntegrationEventPublisher _publisher;
        private readonly ILogger<AcceptResolutionCommandHandler> _logger;

        public AcceptResolutionCommandHandler(IOrderingContext context, IIntegrationEventPublisher publisher, ILogger<AcceptResolutionCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(AcceptResolutionCommand request, CancellationToken cancellationToken)
        {
            var resolution = await _context.Resolutions
                .Include(r => r.Order)
                    .ThenInclude(o => o.Buyer)
                .Include(o => o.Order)
                    .ThenInclude(o => o.OrderStates)
                .FirstOrDefaultAsync(x => x.Version == request.Version.ToString(), cancellationToken: cancellationToken);
            
            if (resolution?.Order == null)
            {
                _logger.LogWarning("Order for resolution with version: {@Version} was not found", request.Version);
                throw new NotFoundException(nameof(Resolution));
            }
            
            if (resolution == null)
            {
                _logger.LogWarning("Resolution with Version: {@Version} was not found", request.Version);
                throw new NotFoundException(nameof(Resolution), request.Version);
            }
            
            if (resolution.Order.OrderStates.All(x => x.OrderState != OrderState.InProgress))
            {
                throw new ValidationException(new []
                {
                    new ValidationFailure("OrderStates", "Order is not marked in progress")
                });
            }
            
            resolution.Order.OrderStates.Add(new State(OrderState.Completed, $"Buyer accepted resolution - {request.Version}"));
            await _context.SaveChangesAsync(cancellationToken);

            var basePrice = resolution.Order.Quantity * resolution.Order.UnitPrice;
            var tax = resolution.Order.TotalPrice - basePrice;
            await _publisher.PublishAsync(new OrderCompletedIntegrationEvent(resolution.Order.OrderNumber,
                basePrice, tax, resolution.Order.Buyer.UserId, resolution.Order.SellerUserId), cancellationToken);
            
            return Unit.Value;
        }
    }
}