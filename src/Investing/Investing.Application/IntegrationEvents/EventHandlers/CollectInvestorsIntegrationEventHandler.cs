using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Common.Core.Interfaces;
using Hive.Gig.Contracts.IntegrationEvents;
using Hive.Investing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.IntegrationEvents.EventHandlers
{
    public class CollectInvestorsIntegrationEventHandler : ICapSubscribe
    {
        private readonly IEmailService _emailService;
        private readonly IInvestingDbContext _context;

        public CollectInvestorsIntegrationEventHandler(IEmailService emailService, IInvestingDbContext context)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        [CapSubscribe(nameof(NotifyInvestorsIntegrationEvent), Group = "hive.investing.investors.notify")]
        public async Task Handle(NotifyInvestorsIntegrationEvent @event)
        {
            var plan = await _context.Plans
                .Include(p => p.Investments.Where(i => i.IsAccepted))
                .ThenInclude(i => i.Investor)
                .FirstOrDefaultAsync(p => p.Id == @event.PlanId);

            if (plan == null)
            {
                return;
            }

            var investorIds = plan.Investments.Select(i => i.Investor.UserId);
            // check emails for all of the ids, send integration event for those that are missing to be send from the identity service/ user management
        }
    }
}