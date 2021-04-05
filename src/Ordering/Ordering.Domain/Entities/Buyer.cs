﻿using System.Collections.Generic;
using Hive.Common.Domain.SeedWork;

namespace Ordering.Domain.Entities
{
    public class Buyer : Entity
    {
        private Buyer()
        {
            Orders = new HashSet<Order>();
        }

        public Buyer(string userId) : this()
        {
            UserId = userId;
        }
        
        public string UserId { get; private set; }

        public ICollection<Order> Orders { get; private set; }
    }
}