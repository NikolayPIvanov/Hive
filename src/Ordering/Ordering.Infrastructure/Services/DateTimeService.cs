using System;
using Ordering.Application.Interfaces;

namespace Ordering.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}