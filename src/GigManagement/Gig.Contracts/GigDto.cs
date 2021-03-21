using System.Collections.Generic;

namespace Gig.Contracts
{
    public class GigDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public ICollection<string> Tags { get; set; }
        
    }
}