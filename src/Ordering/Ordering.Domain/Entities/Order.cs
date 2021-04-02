using System;
using System.Collections.Generic;
using System.Linq;
using Hive.Common.Domain.SeedWork;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Entities
{
    public class Order : Entity
    {
        private Order()
        {
            Resolutions = new HashSet<Resolution>();
            IsClosed = OrderStates.Any(s => s.OrderState == OrderState.Canceled || s.OrderState == OrderState.Declined || s.OrderState == OrderState.Completed);
        }
        
        public Order(decimal price, string requirements, int gigId, int packageId, int buyerId, int sellerId) : this()
        {
            OrderNumber = Guid.NewGuid();
            OrderedAt = DateTime.UtcNow;
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
                
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public int SellerId { get; private init; }
        public int BuyerId { get; private init; }
        
        public Buyer Buyer { get; private init; }
        
        public decimal UnitPrice { get; set; }
        public bool IsClosed { get; private set; }
        
        public int GigId { get; private init; }
        public int PackageId { get; private init; }
        public int RequirementId { get; private set; }
        public Requirement Requirement { get; private init; }
        
        public ICollection<Resolution> Resolutions { get; private set; }
        
        public ICollection<State> OrderStates { get; private set; }
    }
}