using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Orders
{
    public class Requirement : ValueObject
    {
        private Requirement()
        {
        }

        public Requirement(string details) : this()
        {
            Details = details;
        }

        public string Details { get; private init; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Details;
        }
    }
}