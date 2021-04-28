using System;
using System.Collections.Generic;
using System.Linq;
using Hive.Common.Core.SeedWork;

namespace Hive.Investing.Domain.Entities
{
    public class Plan : Entity
    {
        private Plan()
        {
            Investments = new HashSet<Investment>();
            IsFunded = (Investments ?? new List<Investment>()).Any(i => i.IsAccepted);
            IsPublic = false;
        }

        public Plan(int vendorId, string title, string description, DateTime startDate, DateTime? endDate, decimal startingFunds) : this()
        {
            VendorId = vendorId;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            StartingFunds = startingFunds;
            IsFunded = false;
        }
        
        public string Title { get; set; }

        public string Description { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsPublic { get; set; }
        public bool IsFunded { get; private set; }

        public decimal StartingFunds { get; set; }
        
        public int VendorId { get; private set; }
        public Vendor Vendor { get; private set; }
        
        public ICollection<Investment> Investments { get; private set; }
    }
}