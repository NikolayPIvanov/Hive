namespace Hive.Gig.Domain.Entities
{
    using Hive.Common.Domain.SeedWork;
    
    public class GigScope : Entity
    {
        private GigScope()
        {
        }

        public GigScope(string description, int gigId) : this()
        {
            Description = description;
            GigId = gigId;
        }
        
        public string Description { get; set; }

        public int GigId { get; private set; }
        
    }
}