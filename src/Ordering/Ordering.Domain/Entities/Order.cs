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
            Status = new OrderStatus(null);
            IsClosed = false;
        }
        
        public Order(decimal price, string requirements, int gigId, int packageId, string userId, int sellerId) : this()
        {
            UnitPrice = price;
            GigId = gigId;
            PackageId = packageId;
            OrderedBy = userId;
            SellerId = sellerId;
            Requirement = new Requirement(requirements);
        }
        
        public int Id { get; set; }
        
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public int SellerId { get; private init; }
        public string OrderedBy { get; private init; }
        
        public decimal UnitPrice { get; set; }
        public bool IsClosed { get; set; }

        public int OrderStatusId { get; set; }
        public OrderStatus Status { get; set; }
        
        public int GigId { get; private init; }
        public int PackageId { get; private init; }
        public int RequirementId { get; private init; }
        public Requirement Requirement { get; private init; }
    }
}