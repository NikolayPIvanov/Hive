using System;
using System.Collections.Generic;
using MediatR;

namespace Hive.Application.Gigs.Commands.CreateGig
{
    public class PackageDto
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        // TODO: Needs to be Enum
        public string PackageTier { get; set; }
        
        public decimal Price { get; set; }
        
        public DateTime EstimatedDeliveryTime { get; set; }
    }
    
    public class CreateGigCommand : IRequest<int>
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string? Metadata { get; set; }
        
        public string Tags { get; set; }

        public int CategoryId { get; set; }
        
        public List<PackageDto> Packages { get; set; }
    }
}