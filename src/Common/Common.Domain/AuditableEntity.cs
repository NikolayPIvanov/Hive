using System;

namespace Hive.Common.Domain
{
    public abstract class AuditableEntity
    {
        public DateTime Created { get; } = DateTime.UtcNow;

        public string CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string LastModifiedBy { get; set; }
    }
}