using Hive.Common.Core.SeedWork;

namespace Hive.Investing.Domain.Entities
{
    using Hive.Common.Domain.SeedWork;
   
    using System.Collections.Generic;

    public class Investor : Entity
    {
        private Investor()
        {
            Investments = new HashSet<Investment>();
        }
        
        public Investor(string userId) : this()
        {
            UserId = userId;
        }
        
        public string UserId { get; set; }

        public ICollection<Investment> Investments { get; private set; }
    }
}