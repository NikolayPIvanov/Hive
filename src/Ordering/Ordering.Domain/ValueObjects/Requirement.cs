using System.Collections.Generic;
using Hive.Common.Domain.SeedWork;

namespace Ordering.Domain.ValueObjects
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