using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Seller.Domain
{
    public class Gig : AuditableEntity
    {
        public Gig()
        {
            Packages = new List<Package>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string? Metadata { get; set; }

        public string Tags { get; set; }

        public List<Package> Packages { get; set; }

        public string Description { get; set; }

        public List<GigQuestion> Questions { get; set; }
    }
}