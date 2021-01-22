using System;
using System.Collections.Generic;

namespace Domain.Seller
{
    public class Gig
    {
        public Guid Id { get; set; }

        public Guid GigOverviewId { get; set; }
        public GigOverview GigOverview { get; set; }
        
        // options to include freelancers
    }
}