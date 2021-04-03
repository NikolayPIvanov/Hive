using System;
using Hive.Common.Core.Interfaces;

namespace Hive.Investing.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}