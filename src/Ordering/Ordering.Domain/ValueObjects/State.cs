using System;
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
            Reason = reason ?? Reason;
            Created = DateTime.UtcNow;
        }

        public OrderState OrderState { get; private init; }
        public string Reason { get; private init; }
        
        public DateTime Created { get; private init; }
        
        public static State Initial() => new();
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OrderState;
            yield return Reason;
        }
    }
}