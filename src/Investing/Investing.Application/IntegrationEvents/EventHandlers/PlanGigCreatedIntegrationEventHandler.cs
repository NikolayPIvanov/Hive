using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Gig.Contracts.IntegrationEvents;
using Hive.Investing.Application.Interfaces;
using Investing.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Hive.Investing.Application.IntegrationEvents.EventHandlers
{
    public class PlanGigCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IEmailService _emailService;
        private readonly IRedisCacheClient _redisCacheClient;
        private readonly IInvestingDbContext _context;
        private readonly IIntegrationEventPublisher _publisher;

        public PlanGigCreatedIntegrationEventHandler(IEmailService emailService, IRedisCacheClient redisCacheClient, 
            IInvestingDbContext context, IIntegrationEventPublisher publisher)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _redisCacheClient = redisCacheClient ?? throw new ArgumentNullException(nameof(redisCacheClient));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }
        
        [CapSubscribe(nameof(PlanGigCreatedIntegrationEvent), Group = "hive.investing.investors.notify")]
        public async Task Handle(PlanGigCreatedIntegrationEvent @event)
        {
            var plan = await _context.Plans
                .Include(p => p.Investments.Where(i => i.IsAccepted))
                .ThenInclude(i => i.Investor)
                .FirstOrDefaultAsync(p => p.Id == @event.PlanId);

            if (plan == null)
                return;

            plan.GigId = @event.GigId;
            await _context.SaveChangesAsync(default);

            var investorIds = plan.Investments.Select(i => i.Investor.UserId);

            var investorEmailKeys = investorIds.Select(id => $"{id}:email");
            
            var emailsOnHand = await _redisCacheClient.GetDbFromConfiguration().GetAllAsync<string>(investorEmailKeys);

            var idsLeft = investorIds.Except(emailsOnHand.Keys);

            await _emailService.SendAsync(emailsOnHand.Values, "Gig for plan was created", "Gig was created");

            if (idsLeft.Any())
                await _publisher.PublishAsync(new NotifyInvestorsIntegrationEvent(@event.PlanId, @event.GigId, idsLeft));
        }
    }
}