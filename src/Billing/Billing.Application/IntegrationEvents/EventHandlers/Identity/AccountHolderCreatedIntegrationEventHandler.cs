using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Billing.Domain;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.IntegrationEvents.EventHandlers.Identity
{
    public class AccountHolderCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IBillingContext _context;

        public AccountHolderCreatedIntegrationEventHandler(IBillingContext context)
        {
            _context = context;
        }
        
        // TODO: Refactor
        [CapSubscribe(nameof(BuyerCreatedIntegrationEvent))]
        public async Task Handle(BuyerCreatedIntegrationEvent @event)
        {
            var sellerIsRegistered = await _context.AccountHolders.AnyAsync(s => s.UserId == @event.UserId);
            if (!sellerIsRegistered)
            {
                return;
            }

            var accountHolder = new AccountHolder(@event.UserId);
            _context.AccountHolders.Add(accountHolder);
            await _context.SaveChangesAsync(default);
        }
    }
}