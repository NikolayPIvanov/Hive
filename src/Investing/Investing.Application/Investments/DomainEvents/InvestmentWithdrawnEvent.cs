using MediatR;

namespace Hive.Investing.Application.Investments.DomainEvents
{
    public record InvestmentWithdrawnEvent : INotification;
}