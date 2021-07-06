using System.Collections.Generic;
using BuildingBlocks.Core.MessageBus;

namespace Investing.Contracts.IntegrationEvents
{
    public class InvestorsRoi
    {
        public int ResolutionId { get; set; }
        public string InvestorUserId { get; set; }
        public double Roi { get; set; }
    }

    public record InvestorsDataIntegrationEvent(List<InvestorsRoi> Investors) : IntegrationEvent(nameof(InvestorsDataIntegrationEvent));
}