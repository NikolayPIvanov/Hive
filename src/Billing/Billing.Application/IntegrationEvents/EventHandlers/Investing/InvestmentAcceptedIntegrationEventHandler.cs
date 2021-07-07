using System;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Investing.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.IntegrationEvents.EventHandlers
{
    public class InvestmentAcceptedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IBillingDbContext _context;
        private readonly IIntegrationEventPublisher _publisher;
        private readonly ILogger<InvestmentAcceptedIntegrationEventHandler> _logger;

        public InvestmentAcceptedIntegrationEventHandler(IBillingDbContext context,
            IIntegrationEventPublisher publisher,
            ILogger<InvestmentAcceptedIntegrationEventHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _publisher = publisher;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [CapSubscribe(nameof(InvestmentAcceptedIntegrationEvent), Group = "cap.hive.billing")]
        public async Task Handle(InvestmentAcceptedIntegrationEvent @event)
        {
            var investorWallet = await _context.Wallets
                .Include(w => w.AccountHolder)
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.AccountHolder.UserId == @event.InvestorUserId);

            if (investorWallet == null) return;
            
            var vendorWallet = await _context.Wallets
                .Include(w => w.AccountHolder)
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.AccountHolder.UserId == @event.SellerUserId);
            
            if (vendorWallet == null) return;

            if (investorWallet.Balance < @event.Amount)
            {
                return;
            }

            var paymentTransaction = new Transaction(@event.Amount, null, TransactionType.Payment);
            var fundTransaction = new Transaction(@event.Amount, null, TransactionType.Fund);
            
            investorWallet.AddTransaction(paymentTransaction);
            vendorWallet.AddTransaction(fundTransaction);

            await _context.SaveChangesAsync(default);
            
            await _publisher.PublishAsync(new InvestmentAcceptedTransactionIntegrationEvent(@event.InvestmentId));
        }
    }
}