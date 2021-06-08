using System.Collections.Generic;
using Hive.Gig.Application.GigPackages;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record QuestionDto(string Title, string Answer);

    public record ImagePathDto(string Path);

    public record GigScopeDto(int Id, string Description);

    public class GigOverviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ImagePathDto? ImagePath { get; set; }
        public ICollection<decimal> Prices { get; set; }
        public string SellerUserId { get; set; }
        public int? PlanId { get; set; }
    }

    public class GigDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
        public string Category { get; set; }
        
        public string SellerId { get; set; }
        public string SellerUserId { get; set; }
        

        public int? PlanId { get; set; }

        public bool IsDraft { get; set; }
        
        public ImagePathDto? ImagePath { get; set; }

        public ICollection<string> Tags { get; set; }
        
        public ICollection<PackageDto> Packages { get; set; }

        public ICollection<QuestionDto> Questions { get; set; }
    }
}