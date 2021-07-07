using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using Investing.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Contracts.IntegrationEvents;

namespace Hive.Investing.Application.IntegrationEvents.EventHandlers
{
    public class ResolutionAcceptedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IInvestingDbContext _context;
        private readonly IIntegrationEventPublisher _publisher;

        public ResolutionAcceptedIntegrationEventHandler(IInvestingDbContext context, IIntegrationEventPublisher publisher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _publisher = publisher;
        }
        

        [CapSubscribe(nameof(ResolutionAcceptedIntegrationEvent), Group = "hive.investing.resolution.accepted")]
        public async Task Handle(ResolutionAcceptedIntegrationEvent @event)
        {
            var plan = await _context.Plans
                .Include(p => p.Investments.Where(i => i.IsAccepted))
                .ThenInclude(p => p.Investor)
                .FirstOrDefaultAsync(x => x.GigId == @event.GigId);

            if (plan == null) return;

            var investmentsWithRoi = plan.Investments.Select(i => 
                new InvestorsRoi { Roi = i.RoiPercentage, InvestorUserId = i.Investor.UserId, ResolutionId = @event.ResolutionId}).ToList();

            // send data to ordering
            await _publisher.PublishAsync(new InvestorsDataIntegrationEvent(investmentsWithRoi));
        }
    }
}