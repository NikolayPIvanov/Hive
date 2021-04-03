using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;

namespace Hive.Investing.Application.IntegrationEvents.EventHandlers.Identity
{
    public class InvestorCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IInvestingDbContext _context;

        public InvestorCreatedIntegrationEventHandler(IInvestingDbContext context)
        {
            _context = context;
        }
        
        [CapSubscribe(nameof(InvestorCreatedIntegrationEvent), Group = "hive.investing")]
        public async Task Handle(InvestorCreatedIntegrationEvent @event)
        {
            var investor = new Investor(@event.UserId);

            _context.Investors.Add(investor);
            await _context.SaveChangesAsync();
        }
    }
}