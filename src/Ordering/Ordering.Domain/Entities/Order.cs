using System;
using System.Collections.Generic;
using System.Linq;
using Hive.Common.Core.SeedWork;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Entities
{
    public class Order : Entity
    {
        private Order()
        {
            Resolutions = new HashSet<Resolution>();
            IsClosed = IsInClosedState();
        }
        
        public Order(decimal price, int quantity, decimal totalPrice, string requirements, int packageId, int buyerId, string sellerUserId) : this()
        {
            OrderNumber = Guid.NewGuid();
            OrderedAt = DateTime.UtcNow;
            UnitPrice = price;
            Quantity = quantity;
            TotalPrice = totalPrice;
            PackageId = packageId;
            BuyerId = buyerId;
            SellerUserId = sellerUserId;
            Requirement = new Requirement(requirements);
            OrderStates = new HashSet<State>
            {
                State.Initial()
            };
        }
                
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public string SellerUserId { get; private init; }
        public int BuyerId { get; private init; }
        
        public Buyer Buyer { get; private set; }
        public bool IsClosed { get; private init; }
        public Requirement Requirement { get; private init; }
        public int PackageId { get; private init; }

        public decimal UnitPrice { get; private init; }
        public int Quantity { get; private set; }
        public decimal TotalPrice { get; private set; }

        public ICollection<Resolution> Resolutions { get; private set; }
        
        public ICollection<State> OrderStates { get; private set; }

        private bool IsInClosedState()
        {
            var source = OrderStates ??= new HashSet<State>();
            return source.Any(s => s.OrderState == OrderState.Canceled || s.OrderState == OrderState.Declined || s.OrderState == OrderState.Completed);
        }
    }
    
}