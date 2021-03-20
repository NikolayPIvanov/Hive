using System;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Accounts
{
    public class InvestmentContract : AuditableEntity
    {
        public int Id { get; set; }
        
        public int PlanId { get; set; }

        public decimal RoiPercentage { get; set; }

        public decimal Amount { get; set; }
            
        public int InvestorId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}