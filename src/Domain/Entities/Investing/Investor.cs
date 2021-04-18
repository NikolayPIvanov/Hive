using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Investing
{
    public class Investor : AuditableEntity
    {
        private Investor()
        {
            Investments = new();
        }

        public Investor(string userId) : this()
        {
            UserId = userId;
        }

        public string UserId { get; private set; }

        public List<Investment> Investments { get; private set; }
    }
}