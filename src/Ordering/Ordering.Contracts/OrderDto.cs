using System;

namespace Ordering.Contracts
{
    public record OrderDto
    {
        public int Id { get; set; }
        
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public int SellerId { get; private init; }
        public string OrderedBy { get; private init; }
        
        public decimal UnitPrice { get; set; }
        public bool IsClosed { get; set; }

        public string Status { get; set; }
        public string Reason { get; set; }

        public string Requirements { get; set; }
        
        public int GigId { get; private init; }
        public int PackageId { get; private init; }
    }
}