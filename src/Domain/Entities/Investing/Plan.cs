using System;
using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Gigs;

namespace Hive.Domain.Entities.Investing
{
    public record SearchTag(string Value);
    
    public class Plan : AuditableEntity
    {
        private Plan()
        {
            SearchTags = new HashSet<SearchTag>(10);
            Investments = new HashSet<Investment>();
        }

        public Plan(int sellerId, string title, string description, 
            int estimatedReleaseDays, DateTime? estimatedReleaseDate, decimal fundingNeeded) : this()
        {
            SellerId = sellerId;
            Title = title;
            Description = description;
            EstimatedReleaseDays = estimatedReleaseDays;
            EstimatedReleaseDate = estimatedReleaseDate;
            FundingNeeded = fundingNeeded;
            IsFunded = false;
        }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public int EstimatedReleaseDays { get; set; }
        
        public DateTime? EstimatedReleaseDate { get; set; }

        public decimal FundingNeeded { get; set; }

        public bool IsFunded { get; set; }
        
        public int SellerId { get; set; }

        public Seller Seller { get; set; }

        public ICollection<SearchTag> SearchTags { get; private set; }

        public ICollection<Investment> Investments { get; set; }
    }
}