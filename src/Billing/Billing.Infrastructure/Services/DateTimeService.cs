using System;
using Hive.Common.Core.Interfaces;

namespace Billing.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}