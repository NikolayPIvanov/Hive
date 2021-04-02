using System;
using Hive.Common.Core.Interfaces;

namespace Hive.Identity.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}