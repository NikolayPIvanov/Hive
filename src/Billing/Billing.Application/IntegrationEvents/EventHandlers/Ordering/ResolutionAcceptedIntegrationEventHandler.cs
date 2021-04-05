using System;
using System.Linq;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.IntegrationEvents.EventHandlers.Ordering
{
    public class ResolutionAcceptedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IBillingDbContext _context;

        public ResolutionAcceptedIntegrationEventHandler(IBillingDbContext context)
        {
            _context = context;
        }
        
        public async Task Handle()
        {
            var (userId, ordernumber) = ("s", Guid.NewGuid());
            var sellerUserId = "111";
            
            // find transaction for the user that accepted the order
            var transaction = await _context.Transactions.SingleOrDefaultAsync(x => x.UserId == userId && x.OrderNumber == ordernumber);
            
            // if null throw
            // if transaction type is not hold, throw
            // happy path
            transaction.ChangeFromHoldToPayment();

            var sellerAccount = await _context.AccountHolders.SingleOrDefaultAsync(x => x.UserId == sellerUserId);
            
            // add domain event to raise an integration event to investing with the seller's id, orderid, service id, check for plan that correspons to those
            // if there is a plan, raise an


        }
    }
}