using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Contracts.IntegrationEvents.Outbound;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.IntegrationEvents.EventHandlers
{
    public class VendorCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IInvestingDbContext _context;

        public VendorCreatedIntegrationEventHandler(IInvestingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        

        [CapSubscribe(nameof(SellerCreatedIntegrationEvent), Group = "hive.investing.vendor.creation")]
        public async Task Handle(SellerCreatedIntegrationEvent @event)
        {
            var alreadyRegistered = await _context.Vendors.AnyAsync(v => v.UserId == @event.UserId);
            if (alreadyRegistered)
                return;
            
            var vendor = new Vendor(@event.UserId);

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
        }
    }
}