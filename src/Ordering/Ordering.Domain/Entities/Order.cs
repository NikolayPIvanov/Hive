using System;
using System.Collections.Generic;
using Hive.Common.Domain;

namespace Ordering.Domain.Entities
{
    public class Order : AuditableEntity
    {
        private Order() : base()
        {
            OrderNumber = Guid.NewGuid();
            OrderedAt = DateTime.UtcNow;
            IsClosed = false;
        }
        
        public Order(decimal price, string requirements, int gigId, int packageId, int buyerId, int sellerId) : this()
        {
            UnitPrice = price;
            GigId = gigId;
            PackageId = packageId;
            BuyerId = buyerId;
            SellerId = sellerId;
            Requirement = new Requirement(requirements);
            OrderStates = new HashSet<State>
            {
                State.Initial()
            };
        }
        
        public int Id { get; set; }
        
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public int SellerId { get; private init; }
        public int BuyerId { get; private init; }
        
        public Buyer Buyer { get; private init; }
        
        public decimal UnitPrice { get; set; }
        public bool IsClosed { get; set; }
        
        public int GigId { get; private init; }
        public int PackageId { get; private init; }
        public int RequirementId { get; private init; }
        public Requirement Requirement { get; private init; }
        
        public int? ResolutionId { get; set; }
        
        public Resolution Resolution { get; set; }
        
        public ICollection<State> OrderStates { get; set; }
    }
}