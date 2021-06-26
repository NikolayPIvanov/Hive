using BuildingBlocks.Core.MessageBus;
using Hive.Common.Core.SeedWork;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public record BuyerCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(BuyerCreatedIntegrationEvent));
    public record InvestorCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(InvestorCreatedIntegrationEvent));
    public record SellerCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(SellerCreatedIntegrationEvent));
    public record UserCreatedIntegrationEvent(string UserId, string GivenName, string Surname) : IntegrationEvent(nameof(UserCreatedIntegrationEvent));
}