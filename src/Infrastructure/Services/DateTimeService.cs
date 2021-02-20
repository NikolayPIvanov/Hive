using Hive.Application.Common.Interfaces;
using System;

namespace Hive.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
