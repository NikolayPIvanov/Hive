using System;

namespace Hive.Application.Ordering.Orders.Queries
{
    public class StateDto
    {
        public string OrderState { get; set; }
        public string? Reason { get; set; }
    }
}