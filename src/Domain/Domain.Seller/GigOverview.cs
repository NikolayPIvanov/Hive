using System;
using System.Collections.Generic;

namespace Domain.Seller
{
    public class GigOverview
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }

        public Guid CategoryId { get; set; }
        
        public string? Metadata { get; set; }
        
        public List<string> Tags { get; set; }
    }
}