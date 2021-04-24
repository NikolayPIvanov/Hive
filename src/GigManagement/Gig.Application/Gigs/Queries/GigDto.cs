using System.Collections.Generic;
using Hive.Gig.Application.GigPackages;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record QuestionDto(int Id, string Title, string Answer, int GigId);

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

        public ICollection<QuestionDto> Questions { get; set; }
    }
}