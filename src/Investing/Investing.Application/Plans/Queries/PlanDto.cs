using System;
using System.Collections.Generic;

namespace Hive.Investing.Application.Plans.Queries
{
    public class PlanDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int EstimatedReleaseDays { get; set; }
        
        public DateTime? EstimatedReleaseDate { get; set; }

        public decimal FundingNeeded { get; set; }
        
        public int VendorId { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}