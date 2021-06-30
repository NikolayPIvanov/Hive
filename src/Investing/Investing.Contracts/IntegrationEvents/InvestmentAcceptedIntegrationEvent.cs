using BuildingBlocks.Core.MessageBus;

namespace Investing.Contracts.IntegrationEvents
{
    public record InvestmentAcceptedIntegrationEvent(string InvestorUserId, string SellerUserId, int InvestmentId, decimal Amount) : 
        IntegrationEvent(nameof(InvestmentAcceptedIntegrationEvent));
    
    public record InvestmentAcceptedTransactionIntegrationEvent(int InvestmentId) : 
        IntegrationEvent(nameof(InvestmentAcceptedTransactionIntegrationEvent));
}