using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Enums;

namespace Hive.Application.GigPackages.Queries.GetGigPackages
{
    public class PackageDto : IMapFrom<Package>
    {
        public int Id { get; set; }
        
        public PackageTier PackageTier { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public double DeliveryTime { get; set; }
        
        public DeliveryFrequency DeliveryFrequency { get; set; }
        
        public int GigId { get; set; }
    }
}