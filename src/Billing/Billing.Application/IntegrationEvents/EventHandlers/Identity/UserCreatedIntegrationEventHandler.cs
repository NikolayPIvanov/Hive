﻿using System;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using DotNetCore.CAP;
using Hive.Billing.Domain.Entities;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Contracts.IntegrationEvents.Outbound;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.IntegrationEvents.EventHandlers
{
    public class UserCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IBillingDbContext _context;
        private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

        public UserCreatedIntegrationEventHandler(IBillingDbContext context, ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [CapSubscribe(nameof(UserCreatedIntegrationEvent), Group = "cap.hive.billing")]
        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            var accountHolderAlreadyRegistered = await _context.AccountHolders.AnyAsync(s => s.UserId == @event.UserId);
            if (accountHolderAlreadyRegistered)
            {
                _logger.LogWarning("Account holder with {@UserId} already has an account.", @event.UserId);
                return;
            }
            
            var wallet = new Wallet(@event.UserId);
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync(default);
            _logger.LogInformation("Account holder with {@UserId} successfully created.");
        }
    }
}