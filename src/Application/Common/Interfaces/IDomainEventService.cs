using Hive.Domain.Common;
using System.Threading.Tasks;

namespace Hive.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
