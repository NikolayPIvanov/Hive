using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Investing.Application.Interfaces;
using Investing.Contracts.IntegrationEvents;

namespace Hive.Investing.Application.IntegrationEvents.EventHandlers
{
    public class InvestmentTransactionStatusIntegrationEventHandler : ICapSubscribe
    {
        private readonly IInvestingDbContext _context;
        private readonly IIntegrationEventPublisher _integrationEventPublisher;

        public InvestmentTransactionStatusIntegrationEventHandler(IInvestingDbContext context, IIntegrationEventPublisher integrationEventPublisher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _integrationEventPublisher = integrationEventPublisher ?? throw new ArgumentNullException(nameof(integrationEventPublisher));
        }

        [CapSubscribe(nameof(InvestmentAcceptedTransactionIntegrationEvent), Group = "hive.investing.transactions.status")]
        public async Task Handle(InvestmentAcceptedTransactionIntegrationEvent @event)
        {
            var transaction = await _context.Investments.FindAsync(@event.InvestmentId);
            if (transaction == null) return;
            
            transaction.
        }
    }
}