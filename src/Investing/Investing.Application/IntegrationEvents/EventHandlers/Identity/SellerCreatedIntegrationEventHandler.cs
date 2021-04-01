using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;

namespace Hive.Investing.Application.IntegrationEvents.EventHandlers.Identity
{
    public class SellerCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IInvestingDbContext _context;

        public SellerCreatedIntegrationEventHandler(IInvestingDbContext context)
        {
            _context = context;
        }

        [CapSubscribe(nameof(SellerCreatedIntegrationEvent), Group = "hive.investing")]
        public async Task Handle(SellerCreatedIntegrationEvent @event)
        {
            var vendor = new Vendor(@event.UserId);

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
        }
    }
}