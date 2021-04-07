﻿using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Orders;
using Hive.Domain.Enums;

namespace Hive.Domain.Entities.Gigs
{
    public class Package : AuditableEntity
    {
        private Package()
        {
        }

        public Package(string title, string description, decimal price, double deliveryTime, DeliveryFrequency deliveryFrequency,
            int? revisions, RevisionType revisionType, int gigId) : this()
        {
            Title = title;
            Description = description;
            Price = price;
            DeliveryTime = deliveryTime;
            DeliveryFrequency = deliveryFrequency;
            Revisions = revisions;
            RevisionType = revisionType;
            GigId = gigId;
        }
        
        public PackageTier PackageTier { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public double DeliveryTime { get; set; }
        public DeliveryFrequency DeliveryFrequency { get; set; }
        
        public RevisionType RevisionType { get; set; }
        public int? Revisions { get; set; }
        
        public int GigId { get; init; }
    }
}