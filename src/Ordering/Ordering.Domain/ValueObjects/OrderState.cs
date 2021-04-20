using System.Collections.Generic;
using Hive.Common.Domain.SeedWork;
using Ordering.Domain.Enums;

namespace Ordering.Domain.ValueObjects
{
    public class State : ValueObject
    {
        private State()
        {
            OrderState = OrderState.Validation;
            Reason = "In validation";
        }

        public State(OrderState state, string reason) : this()
        {
            OrderState = state;
            Reason = reason ?? "Validating Order";
        }

        public static State Initial() => new();
        public OrderState OrderState { get; private init; }
        public string Reason { get; init; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OrderState;
            yield return Reason;
        }
    }
}