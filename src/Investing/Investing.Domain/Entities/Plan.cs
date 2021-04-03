using System;
using System.Collections.Generic;
using Hive.Common.Domain.SeedWork;

namespace Hive.Investing.Domain.Entities
{
    public record Tag(string Value);
    
    public class Plan : Entity
    {
        private Plan()
        {
            Tags = new HashSet<Tag>(10);
        }

        public Plan(int vendorId, string title, string description, 
            int estimatedReleaseDays, DateTime? estimatedReleaseDate, decimal fundingNeeded) : this()
        {
            VendorId = vendorId;
            Title = title;
            Description = description;
            EstimatedReleaseDays = estimatedReleaseDays;
            EstimatedReleaseDate = estimatedReleaseDate;
            FundingNeeded = fundingNeeded;
        }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public int EstimatedReleaseDays { get; set; }
        
        public DateTime? EstimatedReleaseDate { get; set; }

        public decimal FundingNeeded { get; set; }
        
        public int VendorId { get; set; }

        public ICollection<Tag> Tags { get; private set; }
    }
}