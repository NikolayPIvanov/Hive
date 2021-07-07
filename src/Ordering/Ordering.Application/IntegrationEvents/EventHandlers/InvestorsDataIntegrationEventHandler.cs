using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Investing.Contracts.IntegrationEvents;
using MediatR;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.IntegrationEvents.EventHandlers
{
    public class InvestorsDataIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;
        private readonly IIntegrationEventPublisher _publisher;

        public InvestorsDataIntegrationEventHandler(IOrderingContext context, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        [CapSubscribe(nameof(InvestorsDataIntegrationEvent), Group = "hive.ordering.investors.roi")]
        public async Task Handle(InvestorsDataIntegrationEvent @event)
        {
            var resolutionId = @event.Investors.First().ResolutionId;
            var resolution = await _context.Resolutions
                .Include(r => r.Order)
                .ThenInclude(o => o.Buyer)
                .Include(o => o.Order)
                .ThenInclude(o => o.OrderStates)
                .FirstOrDefaultAsync(x => x.Id == resolutionId);

            if (resolution == null) return;
            
            var netPrice = resolution.Order.Quantity * resolution.Order.UnitPrice;
            var tax = resolution.Order.TotalPrice - netPrice;

            var amountForInvestors = 0.0m;
            var depositData = new List<WalletDataDeposit>();

            foreach (var data in @event.Investors)
            {
                var amount = netPrice * (decimal)data.Roi / 100.0m;
                depositData.Add(new WalletDataDeposit(data.InvestorUserId, amount, resolution.Order.OrderNumber.ToString()));
                amountForInvestors += amount;
            }
            
            // for seller
            depositData.Add(new WalletDataDeposit(
                resolution.Order.SellerUserId, 
                netPrice - amountForInvestors < 0.0m ? 0.0m : netPrice - amountForInvestors, 
                resolution.Order.OrderNumber.ToString()));
            
            await _publisher.PublishAsync(new OrderCompletedIntegrationEvent(
                resolution.Order.OrderNumber,
                resolution.Id,
                resolution.Order.Buyer.UserId, 
                resolution.Order.SellerUserId,
                netPrice, tax, depositData));
        }
    }
}