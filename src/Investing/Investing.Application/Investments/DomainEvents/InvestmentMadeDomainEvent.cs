using MediatR;

namespace Hive.Investing.Application.Investments.DomainEvents
{
    // TODO: Notify the vendor that an investment offer was created
    public record InvestmentMadeDomainEvent(int PlanId, string VendorUserId) : INotification;
    
}