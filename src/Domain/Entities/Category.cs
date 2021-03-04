using Hive.Domain.Common;

namespace Hive.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
    }
}