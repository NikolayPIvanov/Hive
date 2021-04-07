namespace Hive.Application.Ordering.Orders.Queries
{
    public class ResolutionDto
    {
        public string Version { get; init; }

        public string Location { get; init; }

        public int OrderId { get; init; }

        public bool IsApproved { get; set; }
    }
}