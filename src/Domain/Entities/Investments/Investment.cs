using System;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Investments
{
    public class Investment : AuditableEntity
    {
        public Investment()
        {
            StartDate = DateTime.UtcNow;
        }
        
        public int Id { get; set; }
        
        public int PlanId { get; set; }
        
        public Plan Plan { get; set; }

        public decimal RoiPercentage { get; set; }

        public decimal Amount { get; set; }
            
        public int InvestorId { get; set; }

        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; set; }
    }
}