using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Gigs
{
    public class GigScope : ValueObject
    {
        private GigScope()
        {
        }

        public GigScope(string description) : this()
        {
            Description = description;
        }
        
        public string Description { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
        }
    }
}