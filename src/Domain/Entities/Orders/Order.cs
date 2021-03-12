using System;
using Hive.Domain.Common;
using Hive.Domain.Enums;

namespace Hive.Domain.Entities.Orders
{
    public class Order : AuditableEntity
    {
        public Order()
        {
            OrderNumber = Guid.NewGuid();
            OrderedAt = DateTime.UtcNow;
            IsCanceled = false;
            Status = OrderStatus.Pending;
        }
        
        public int Id { get; set; }
        
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public bool IsCanceled { get; private set; }
        public string? CanceledBy { get; private set; }
        // TODO: Would be good if it is with private set
        public string OrderedById { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime? AcceptedAt { get; private set; } = null;
        
        
        public int OfferedById { get; set; }

        public decimal TotalAmount { get; set; }
        
        public int GigId { get; set; }
        
        public int PackageId { get; set; }
        
        // TODO: Gig Extras

        public void Cancel(string canceledBy)
        {
            IsCanceled = true;
            CanceledBy = canceledBy;
        }
        
        public void Accept()
        {
            AcceptedAt = DateTime.UtcNow;
            Status = OrderStatus.InProgress;
        }
        
        public void Decline()
        {
            
        }
        
    }
}