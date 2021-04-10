using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Investing
{
    public class Investor : AuditableEntity
    {
        public Investor()
        {
            Investments = new();
        }

        public string UserId { get; set; }

        public List<Investment> Investments { get; private set; }
    }
}