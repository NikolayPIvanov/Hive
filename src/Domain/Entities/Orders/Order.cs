using System;
using System.Collections.Generic;
using System.Linq;
using Hive.Domain.Common;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Enums;
using Hive.Domain.ValueObjects;

namespace Hive.Domain.Entities.Orders
{
    public class Order : AuditableEntity
    {
        private Order()
        {
            Resolutions = new HashSet<Resolution>();
            IsClosed = OrderStates.Any(s => s.OrderState == OrderState.Canceled || s.OrderState == OrderState.Declined || s.OrderState == OrderState.Completed);
        }
        
        public Order(decimal price, string requirements, int packageId, int buyerId, int sellerId) : this()
        {
            OrderNumber = Guid.NewGuid();
            OrderedAt = DateTime.UtcNow;
            UnitPrice = price;
            PackageId = packageId;
            BuyerId = buyerId;
            //SellerId = sellerId;
            Requirement = new Requirement(requirements);
            OrderStates = new HashSet<State>
            {
                State.Initial()
            };
        }
                
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public Requirement Requirement { get; set; }

        public decimal UnitPrice { get; set; }
        public bool IsClosed { get; private set; }

        public int PackageId { get; private init; }

        public Package Package { get; set; }
        
        // public int SellerId { get; private init; }
        // public Seller Seller { get; set; }
        
        public int BuyerId { get; private init; }
        
        public Buyer Buyer { get; private init; }
        
        public ICollection<Resolution> Resolutions { get; private set; }
        
        public ICollection<State> OrderStates { get; private set; }
    }
}