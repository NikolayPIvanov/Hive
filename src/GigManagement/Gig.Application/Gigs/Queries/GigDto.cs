using System.Collections.Generic;
using Hive.Gig.Application.GigPackages;
using Hive.Gig.Contracts.Objects;

namespace Hive.Gig.Application.Gigs.Queries
{
    public class GigDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }
        
        public string SellerId { get; set; }

        public ICollection<string> Tags { get; set; }
        
        public ICollection<PackageDto> Packages { get; set; }
    }
}