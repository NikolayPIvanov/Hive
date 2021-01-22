using System;

namespace Domain.Seller
{
    public class Plan
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal FundingNeeded { get; set; }
        
        public TimeSpan? ExpectedDuration { get; set; }
    }
}