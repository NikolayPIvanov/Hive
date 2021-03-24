using Hive.Common.Domain;

namespace Ordering.Domain.Entities
{
    public class OrderStatus : AuditableEntity
    {
        private OrderStatus()
        {
            Status = Enums.OrderStatus.InValidation;
            Reason ??= "Validating Order";
        }

        public OrderStatus(string? reason) : this()
        {
            Reason = reason;
        }
        
        public int Id { get; set; }

        public Enums.OrderStatus Status { get; set; }

        public string? Reason { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}