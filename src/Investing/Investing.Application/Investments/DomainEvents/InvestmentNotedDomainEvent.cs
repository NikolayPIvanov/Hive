using MediatR;

namespace Hive.Investing.Application.Investments.DomainEvents
{
    // TODO: Notify the investor that an investment offer was noted
    public record InvestmentNotedDomainEvent : INotification;
}