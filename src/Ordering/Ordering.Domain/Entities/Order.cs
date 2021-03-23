using System;
using Hive.Common.Domain;

namespace Ordering.Domain.Entities
{
    public class Order : AuditableEntity
    {
        private Order()
        {
            OrderNumber = Guid.NewGuid();
            OrderedAt = DateTime.UtcNow;
        }
        
        public Order(decimal price, int gigId, int packageId) : this()
        {
            UnitAmount = price;
        }
        
        public int Id { get; set; }
        
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public decimal UnitAmount { get; private init; }
        
        public int GigId { get; set; }
        
        public int PackageId { get; set; }
        
        public int RequirementId { get; set; }
        
        public Requirement Requirement { get; set; }
    }
}