using Hive.Common.Domain;
using Ordering.Domain.Enums;

namespace Ordering.Domain.Entities
{
    public class State : AuditableEntity
    {
        private State()
        {
            OrderState = OrderState.Validation;
            Reason ??= "Validating Order";
        }

        public State(OrderState state, string reason) : this()
        {
            OrderState = state;
            Reason = reason;
        }

        public static State Initial() => new();
        
        public int Id { get; set; }

        public OrderState OrderState { get; set; }

        public string? Reason { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}