using System;

namespace Hive.Seller.Domain
{
    public class Gig
    {
        public Gig(Guid sellerId)
        {
            IsDraft = true;
            SellerId = sellerId;
        }
        
        public Guid Id { get; set; }

        public Guid GigOverviewId { get; set; }
        public GigOverview GigOverview { get; set; }

        public Guid SellerId { get; set; }
        public Seller Seller { get; set; }

        public bool IsDraft { get; set; }
        
        // options to include freelancers
    }
}