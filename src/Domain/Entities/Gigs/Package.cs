﻿using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Orders;
using Hive.Domain.Enums;

namespace Hive.Domain.Entities.Gigs
{
    public class Package : AuditableEntity
    {
        public Package()
        {
            Orders = new();
        }
        
        public int Id { get; set; }
        
        public PackageTier PackageTier { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public double DeliveryTime { get; set; }
        
        public DeliveryFrequency DeliveryFrequency { get; set; }
        
        public int GigId { get; set; }

        public Gig Gig { get; set; }

        public List<Order> Orders { get; set; }
    }
}