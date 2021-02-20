using Hive.Domain.Common;

namespace Hive.Seller.Domain
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
    }
}