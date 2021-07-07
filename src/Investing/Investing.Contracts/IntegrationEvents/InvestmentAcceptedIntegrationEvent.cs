using BuildingBlocks.Core.MessageBus;

namespace Investing.Contracts.IntegrationEvents
{
    // Request for wallet validation
    public record InvestmentAcceptedIntegrationEvent(string InvestorUserId, string SellerUserId, int InvestmentId, decimal Amount) : 
        IntegrationEvent(nameof(InvestmentAcceptedIntegrationEvent));
    
    // Set status to accepted
    public record InvestmentAcceptedTransactionIntegrationEvent(int InvestmentId) : 
        IntegrationEvent(nameof(InvestmentAcceptedTransactionIntegrationEvent));
}