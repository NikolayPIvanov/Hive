using System;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Orders
{
    public class Requirement : AuditableEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public string Details { get; set; }
    }
}