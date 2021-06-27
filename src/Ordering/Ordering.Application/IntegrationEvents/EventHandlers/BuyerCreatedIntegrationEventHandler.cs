using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Contracts.IntegrationEvents.Inbound;
using Hive.Identity.Contracts.IntegrationEvents.Outbound;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.EventHandlers
{
    public class BuyerCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;
        private readonly IIntegrationEventPublisher _publisher;

        public BuyerCreatedIntegrationEventHandler(IOrderingContext context, IIntegrationEventPublisher publisher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }
        
        [CapSubscribe(nameof(BuyerCreatedIntegrationEvent), Group = "hive.ordering.buyers")]
        public async Task Handle(InvestorCreatedIntegrationEvent @event)
        {
            var alreadyRegistered = await _context.Buyers.AnyAsync(v => v.UserId == @event.UserId);
            if (alreadyRegistered)
                return;
            
            var buyer = new Buyer(@event.UserId);

            _context.Buyers.Add(buyer);
            await _context.SaveChangesAsync(default);

            await _publisher.PublishAsync(
                new ExternalAccountSetIntegrationEvent(@event.UserId, buyer.Id));
        }
    }
}