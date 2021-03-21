using Hive.Common.Domain;
using Hive.Gig.Domain.Enums;

namespace Hive.Gig.Domain.Entities
{
    public class Package : AuditableEntity
    {
        public int Id { get; set; }
        
        public PackageTier PackageTier { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public double DeliveryTime { get; set; }
        
        public DeliveryFrequency DeliveryFrequency { get; set; }

        public int Revisions { get; set; }
        
        public int GigScopeId { get; set; }

        public GigScope GigScope { get; set; }
    }
}