using System.Collections.Generic;
using Hive.Common.Domain;

namespace Hive.Gig.Domain.Entities
{
    public class GigScope : AuditableEntity
    {
        private GigScope()
        {
            //Questions = new HashSet<Question>();
        }

        public GigScope(string description, int gigId)
        {
            Description = description;
            GigId = gigId;
        }
        
        public int Id { get; set; }

        public string Description { get; set; }

        public int GigId { get; set; }

        public Gig Gig { get; set; }
        
    }
}