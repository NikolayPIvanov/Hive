using Hive.Common.Domain;

namespace Hive.Gig.Domain.Entities
{
    public class Gig : AuditableEntity
    {
        // Numeric Ids take less space in tables
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        
        public int CategoryId { get; set; }
        // TODO: Add Navigational Property if needed
    }
}