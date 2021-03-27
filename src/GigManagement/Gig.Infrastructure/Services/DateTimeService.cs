using System;
using Hive.Gig.Application.Interfaces;

namespace Hive.Gig.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
