using System;
using Hive.Domain.Common;
using Hive.Domain.Enums;

namespace Hive.Domain.Entities.Gigs
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
        
        public int GigId { get; set; }

        public Gig Gig { get; set; }
    }
}