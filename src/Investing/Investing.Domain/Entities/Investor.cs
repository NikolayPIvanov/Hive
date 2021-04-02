using Hive.Common.Domain.SeedWork;

namespace Hive.Investing.Domain.Entities
{
    using Hive.Common.Domain;
    
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
        
        public int Id { get; set; }

        public string UserId { get; set; }

        public ICollection<Investment> Investments { get; private set; }
    }
}