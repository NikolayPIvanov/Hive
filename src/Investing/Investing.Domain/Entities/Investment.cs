using System;
using Hive.Common.Core.SeedWork;

namespace Hive.Investing.Domain.Entities
{
    public class Investment : Entity
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
        }
        
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public decimal Amount { get; set; }
        public double RoiPercentage { get; set; }

        public int InvestorId { get; private set; }
        public Investor Investor { get; set; }

        public int PlanId { get; private set; }
        public Plan Plan { get; set; }
        
        public bool IsAccepted { get; set; }
    }
}