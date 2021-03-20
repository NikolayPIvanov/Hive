using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Investments
{
    public class Investor : AuditableEntity
    {
        public Investor()
        {
            Investments = new();
        }
        
        public int Id { get; set; }

        public string UserId { get; set; }

        public List<Investment> Investments { get; private set; }
    }
}