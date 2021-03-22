namespace Gig.Contracts.IntegrationEvents
{
    public record CategoryCreatedIntegrationEvent(int CategoryId, string Title) 
        : IntegrationEvent(nameof(CategoryCreatedIntegrationEvent));
}