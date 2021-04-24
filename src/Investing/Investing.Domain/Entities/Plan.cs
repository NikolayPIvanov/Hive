using System;
using System.Collections.Generic;
using System.Linq;
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
            IsFunded = (Investments ?? new List<Investment>()).Any(i => i.IsAccepted);
        }

        public Plan(int vendorId, string title, string description, DateTime estimatedReleaseDate, decimal startingFunds) : this()
        {
            VendorId = vendorId;
            Title = title;
            Description = description;
            EstimatedReleaseDate = estimatedReleaseDate;
            StartingFunds = startingFunds;
            IsFunded = false;
        }
        
        public string Title { get; set; }

        public string Description { get; set; }
        
        public DateTime EstimatedReleaseDate { get; set; }

        public decimal StartingFunds { get; set; }

        public bool IsFunded { get; private set; }
        
        public int VendorId { get; private set; }
        public Vendor Vendor { get; private set; }

        public ICollection<SearchTag> SearchTags { get; private set; }

        public ICollection<Investment> Investments { get; private set; }
    }
}