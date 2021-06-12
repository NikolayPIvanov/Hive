using System;
using System.Collections.Generic;

namespace Hive.Investing.Application.Plans.Queries
{
    public class PlanDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsFunded { get; set; }

        public bool IsPublic { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public decimal FundingNeeded { get; set; }
        
        public int VendorId { get; set; }
        
        public string VendorUserId { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}