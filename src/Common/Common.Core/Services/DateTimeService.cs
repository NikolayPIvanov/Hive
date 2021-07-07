using System;
using Hive.Common.Core.Interfaces;

namespace Hive.Common.Core.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}