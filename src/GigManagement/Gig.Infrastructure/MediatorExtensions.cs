using System.Linq;
using System.Threading.Tasks;
using Hive.Common.Core.SeedWork;
using Hive.Gig.Application.Questions.Interfaces;
using Hive.Gig.Infrastructure.Persistence;
using MediatR;

namespace Hive.Gig.Infrastructure
{
    public  static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, GigManagementDbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}