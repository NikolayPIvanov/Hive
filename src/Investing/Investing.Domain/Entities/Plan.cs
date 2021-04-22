﻿using System;
using System.Collections.Generic;
using Hive.Common.Core.SeedWork;

namespace Hive.Investing.Domain.Entities
{
    public record SearchTag(string Value);
    
    public class Plan : Entity
    {
        private Plan()
        {
            SearchTags = new HashSet<SearchTag>(10);
            Investments = new HashSet<Investment>();
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
            IsFunded = false;
        }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public int EstimatedReleaseDays { get; set; }
        
        public DateTime? EstimatedReleaseDate { get; set; }

        public decimal FundingNeeded { get; set; }

        public bool IsFunded { get; set; }
        
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public ICollection<SearchTag> SearchTags { get; private set; }

        public ICollection<Investment> Investments { get; set; }
    }
}