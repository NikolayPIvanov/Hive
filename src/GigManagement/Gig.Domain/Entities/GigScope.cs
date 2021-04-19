using System.Collections.Generic;

namespace Hive.Gig.Domain.Entities
{
    using Hive.Common.Domain.SeedWork;
    
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