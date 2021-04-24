using Hive.Common.Core.SeedWork;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public class ConformationEvents
    {
        public record BuyerStoredIntegrationEvent
            (string UserId, int BuyerId, bool IsSuccess) : IntegrationEvent(nameof(BuyerStoredIntegrationEvent));
        
        public record SellerStoredIntegrationEvent
            (string UserId, int SellerId, bool IsSuccess) : IntegrationEvent(nameof(SellerStoredIntegrationEvent));
        
        public record InvestorStoredIntegrationEvent
            (string UserId, int InvestorId, bool IsSuccess) : IntegrationEvent(nameof(InvestorStoredIntegrationEvent));
    }
}