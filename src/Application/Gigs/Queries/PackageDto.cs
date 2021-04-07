namespace Hive.Application.Gigs.Queries
{
    public class PackageDto
    {
        public int Id { get; set; }
        
        public string PackageTier { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public double DeliveryTime { get; set; }
        
        public string DeliveryFrequency { get; set; }

        public int Revisions { get; set; }
        
        public int GigId { get; set; }
    }
}