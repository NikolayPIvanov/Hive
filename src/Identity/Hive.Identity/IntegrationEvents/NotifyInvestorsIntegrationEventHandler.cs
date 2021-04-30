using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Data;
using Investing.Contracts.IntegrationEvents;

namespace Hive.Identity.IntegrationEvents
{
    public class NotifyInvestorsIntegrationEventHandler : ICapSubscribe
    {
        private readonly ApplicationDbContext _context;

        public NotifyInvestorsIntegrationEventHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [CapSubscribe(nameof(NotifyInvestorsIntegrationEvent))]
        public async Task Handle(NotifyInvestorsIntegrationEvent @event)
        {
            // TODO:
        }
    }
}