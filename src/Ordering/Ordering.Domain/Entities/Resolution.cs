using Hive.Common.Core.SeedWork;

namespace Ordering.Domain.Entities
{
    public class Resolution : Entity
    {
        private Resolution()
        {
        }

        public Resolution(string version, string location, int orderId) : this()
        {
            Version = version;
            Location = location;
            OrderId = orderId;
            IsApproved = false;
        }
        
        public string Version { get; set; }

        public string Location { get; set; }

        public int OrderId { get; private init; }
        public Order Order { get; set; }

        public bool IsApproved { get; set; }
    }
}