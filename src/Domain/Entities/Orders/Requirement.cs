using System;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Orders
{
    public class Requirement : AuditableEntity
    {
        private Requirement()
        {
        }

        public Requirement(string details) : this()
        {
            Details = details;
        }
                
        public string Details { get; set; }

        public int OrderId { get; set; }
    }
}