using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Accounts
{
    public class Investor : AuditableEntity
    {
        public Investor()
        {
            Investments = new();
        }
        
        public int Id { get; set; }

        public List<InvestmentContract> Investments { get; private set; }
    }
}