using System.Collections.Generic;
using BuildingBlocks.Core.MessageBus;

namespace Investing.Contracts.IntegrationEvents
{
    public record NotifyInvestorsIntegrationEvent(int PlanId, int GigId, IEnumerable<string> InvestorsIds) :
        IntegrationEvent(nameof(NotifyInvestorsIntegrationEvent));
}