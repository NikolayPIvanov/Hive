using System;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Gigs
{
    public class Package : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        // TODO: Needs to be Enum
        public string PackageTier { get; set; }
        
        public decimal Price { get; set; }
        
        public DateTime EstimatedDeliveryTime { get; set; }
        
        public int GigId { get; set; }

        public Gig Gig { get; set; }
    }
}