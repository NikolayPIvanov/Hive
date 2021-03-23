using Hive.Common.Domain;

namespace Gig.Contracts.IntegrationEvents
{
    public record CategoryCreated(int CategoryId, string Title) 
        : IntegrationEvent(nameof(CategoryCreated));
    
    public record CategoryUpdated(int CategoryId, string Title) 
        : IntegrationEvent(nameof(CategoryUpdated));

    public record CategoryDelete(int CategoryId, string Title)
        : IntegrationEvent(nameof(CategoryUpdated));
}