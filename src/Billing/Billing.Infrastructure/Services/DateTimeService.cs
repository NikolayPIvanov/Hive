using System;
using Billing.Application.Interfaces;

namespace Billing.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}