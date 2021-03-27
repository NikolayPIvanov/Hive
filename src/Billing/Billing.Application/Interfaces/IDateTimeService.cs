using System;

namespace Billing.Application.Interfaces
{
    public interface IDateTimeService
    {
        public DateTime Now { get; }
    }
}