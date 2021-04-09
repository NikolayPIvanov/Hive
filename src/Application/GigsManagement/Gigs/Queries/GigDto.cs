using System.Collections.Generic;
using Hive.Application.GigsManagement.GigPackages.Queries;

namespace Hive.Application.GigsManagement.Gigs.Queries
{
    public record GigScopeDto(int Id, string Description);
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