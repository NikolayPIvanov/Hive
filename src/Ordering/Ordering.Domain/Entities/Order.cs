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
            IsClosed =  IsInClosedState();
        }
        
        public Order(decimal price, string requirements, int packageId, string buyerId, string sellerId) : this()
        {
            OrderNumber = Guid.NewGuid();
            OrderedAt = DateTime.UtcNow;
            UnitPrice = price;
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
        public string SellerId { get; private init; }
        public string BuyerId { get; private init; }
        public bool IsClosed { get; private init; }
        public Requirement Requirement { get; private init; }
        public int PackageId { get; private init; }

        public decimal UnitPrice { get; private init; }

        public ICollection<Resolution> Resolutions { get; private set; }
        
        public ICollection<State> OrderStates { get; private set; }

        private bool IsInClosedState()
        {
            var source = OrderStates ??= new HashSet<State>();
            return source.Any(s => s.OrderState == OrderState.Canceled || s.OrderState == OrderState.Declined || s.OrderState == OrderState.Completed);
        }
    }
    
}