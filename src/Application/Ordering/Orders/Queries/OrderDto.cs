using System;
using System.Collections.Generic;
using Hive.Application.Ordering.Resolutions.Queries;

namespace Hive.Application.Ordering.Orders.Queries
{
    public record OrderDto
    {
        public int Id { get; set; }
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public int SellerId { get; private init; }
        public int BuyerId { get; private init; }
        
        public decimal UnitPrice { get; set; }
        public bool IsClosed { get; set; }
        
        public int GigId { get; private init; }
        public int PackageId { get; private init; }
        public RequirementDto Requirement { get; set; }
        public ICollection<StateDto> OrderStates { get; set; }
        public ICollection<ResolutionDto> Resolutions { get; set; }
    }
}