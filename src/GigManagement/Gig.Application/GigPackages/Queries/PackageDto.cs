using Hive.Gig.Domain.Enums;

namespace Hive.Gig.Application.GigPackages
{
    public class PackageDto
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public PackageTier PackageTier { get; set; }

        public double DeliveryTime { get; set; }
        
        public DeliveryFrequency DeliveryFrequency { get; set; }

        public string Revisions { get; set; }
        
        public RevisionType RevisionType { get; set; }

        public int GigId { get; set; }
    }
}