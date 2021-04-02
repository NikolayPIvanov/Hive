using System;
using Hive.Common.Core.Interfaces;

namespace Ordering.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}