using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hive.Seller.Domain
{
    public class GigOverview
    {
        public GigOverview() { }
        public GigOverview(string title, Guid categoryId, IEnumerable<string> tags) : this()
        {
            Title = title;
            CategoryId = categoryId;
            Tags = string.Join(",", tags);
        }
        
        public Guid Id { get; set; }
        
        public string Title { get; set; }

        public Guid CategoryId { get; set; }
        
        public string? Metadata { get; set; }
        
        public string Tags { get; set; }

        public Gig Gig { get; set; }

        public Guid GigId { get; set; }
    }
}