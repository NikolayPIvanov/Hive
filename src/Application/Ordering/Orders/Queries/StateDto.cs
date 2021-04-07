using System;

namespace Hive.Application.Ordering.Orders.Queries
{
    public record StateDto
    {
        public int Id { get; set; }

        public string OrderState { get; set; }

        public string? Reason { get; set; }

        public DateTime Created { get; set; }
        
        public string CreatedBy { get; set; }
    }
}