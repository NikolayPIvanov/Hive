using System;
using System.Collections.Generic;

namespace Domain.Seller
{
    public class Vendor
    {
        public Guid Id { get; set; }

        public List<Plan> Plans { get; set; }
    }
}