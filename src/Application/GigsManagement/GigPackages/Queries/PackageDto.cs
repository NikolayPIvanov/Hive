using Hive.Domain.Enums;

namespace Hive.Application.GigsManagement.GigPackages.Queries
{
    public class PackageDto
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