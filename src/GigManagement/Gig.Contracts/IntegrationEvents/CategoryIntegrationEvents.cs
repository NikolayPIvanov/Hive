using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    public record CategoryCreated(int CategoryId, string Title) 
        : IntegrationEvent(nameof(CategoryCreated));
    
    public record CategoryUpdated(int CategoryId, string Title) 
        : IntegrationEvent(nameof(CategoryUpdated));

    public record CategoryDelete(int CategoryId, string Title)
        : IntegrationEvent(nameof(CategoryUpdated));
}