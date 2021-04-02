using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;
using Hive.Gig.Domain.Enums;

namespace Hive.Gig.Domain.Entities
{
    public class Package : Entity
    {
        public int Id { get; set; }
        
        public PackageTier PackageTier { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public double DeliveryTime { get; set; }
        
        public DeliveryFrequency DeliveryFrequency { get; set; }

        public int Revisions { get; set; }
        
        public int GigId { get; set; }

        public Gig Gig { get; set; }
    }
}