﻿using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Contracts.IntegrationEvents.Inbound;
using Hive.Identity.Contracts.IntegrationEvents.Outbound;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.IntegrationEvents.EventHandlers
{
    public class InvestorCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IInvestingDbContext _context;
        private readonly IIntegrationEventPublisher _integrationEventPublisher;

        public InvestorCreatedIntegrationEventHandler(IInvestingDbContext context, IIntegrationEventPublisher integrationEventPublisher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _integrationEventPublisher = integrationEventPublisher ?? throw new ArgumentNullException(nameof(integrationEventPublisher));
        }
        
        [CapSubscribe(nameof(InvestorCreatedIntegrationEvent), Group = "hive.investing.investor.created")]
        public async Task Handle(InvestorCreatedIntegrationEvent @event)
        {
            var alreadyRegistered = await _context.Investors.AnyAsync(v => v.UserId == @event.UserId);
            if (alreadyRegistered)
                return;
            
            var investor = new Investor(@event.UserId);

            _context.Investors.Add(investor);
            await _context.SaveChangesAsync();

            await _integrationEventPublisher.PublishAsync(new ExternalAccountSetIntegrationEvent(@event.UserId, investor.Id));
        }
    }
}