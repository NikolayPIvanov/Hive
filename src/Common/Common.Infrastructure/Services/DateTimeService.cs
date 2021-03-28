using System;
using Hive.Common.Application.Interfaces;

namespace Common.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
