using System;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Investing
{
    public class Investment : AuditableEntity
    {
        private Investment()
        {
        }

        public Investment(DateTime effectiveDate, DateTime? expirationDate, decimal amount, double roiPercentage, int investorId, int planId) : this()
        {
            EffectiveDate = effectiveDate;
            ExpirationDate = expirationDate;
            Amount = amount;
            RoiPercentage = roiPercentage;
            InvestorId = investorId;
            PlanId = planId;
            IsAccepted = false;
        }
        
        public DateTime EffectiveDate { get; set; }
        
        public DateTime? ExpirationDate { get; set; }

        public decimal Amount { get; set; }
        
        public double RoiPercentage { get; set; }

        public int InvestorId { get; set; }

        public Investor Investor { get; set; }

        public int PlanId { get; set; }

        public Plan Plan { get; set; }

        public bool IsAccepted { get; set; }
    }
}