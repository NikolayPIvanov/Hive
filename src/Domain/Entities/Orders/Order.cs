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
            Status = OrderStatus.Pending;
            IsCanceled = false;
        }
        
        public int Id { get; set; }
        
        public Guid OrderNumber { get; private init; }
        public DateTime OrderedAt { get; private init; }
        public string OrderedById { get; set; }
        public bool IsCanceled { get; private set; }
        public OrderStatus Status { get; set; }
        public int OfferedById { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        public DateTime? AcceptedAt { get; private set; } = null;
        public DateTime? DeclinedAt { get; private set; } = null;
        public string CanceledBy { get; private set; } = null;
        
        
        public int GigId { get; set; }
        
        public int PackageId { get; set; }
        
        public int RequirementId { get; set; }

        public Requirement Requirement { get; set; }
        
        
        // TODO: Gig Extras

        public void SetInProgress()
        {
            Status = OrderStatus.InProgress;
        }
        
        public void Cancel(string canceledBy)
        {
            IsCanceled = true;
            CanceledBy = canceledBy;
        }
        
        public void Accept()
        {
            AcceptedAt = DateTime.UtcNow;
            Status = OrderStatus.Accepted;
        }
        
        public void Decline()
        {
            DeclinedAt = DateTime.UtcNow;
            Status = OrderStatus.Declined;
        }
        
    }
}