using System;
using System.Collections.Generic;

namespace Ordering.Contracts
{
    public record StateDto
    {
        public int Id { get; set; }

        public string OrderState { get; set; }

        public string? Reason { get; set; }

        public DateTime Created { get; set; }
        
        public string CreatedBy { get; set; }
    }
    
    public record OrderDto
    {
        public int Id { get; set; }
        
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public int SellerId { get; private init; }
        public string OrderedBy { get; private init; }
        
        public decimal UnitPrice { get; set; }
        public bool IsClosed { get; set; }

        public string Requirements { get; set; }
        
        public int GigId { get; private init; }
        public int PackageId { get; private init; }
        public int RequirementId { get; private init; }
        public int ResolutionId { get; set; }

        public ICollection<StateDto> OrderStates { get; set; }
    }
}