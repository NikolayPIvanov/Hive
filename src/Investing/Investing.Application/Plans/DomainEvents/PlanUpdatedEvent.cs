using System.Threading;
using System.Threading.Tasks;
using Hive.Investing.Application.Plans.Commands;
using MediatR;

namespace Hive.Investing.Application.Plans.DomainEvents
{
    public record PlanUpdatedDomainEvent(int PlanId, UpdatePlanCommand UpdatedPlanCommand) : INotification;

    public class PlanUpdatedEventHandler : INotificationHandler<PlanUpdatedDomainEvent>
    {
        public Task Handle(PlanUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Get investor ids that are interested in investing in this plan from Redis?
            // TODO: Send integration event for the emails of the interested investors or get from Redis?
            // TODO: Get email service
            // TODO: Send email to the investors

            return Task.CompletedTask;
        }
    }
}