namespace Hive.Application.Ordering.Resolutions.Queries
{
    public class ResolutionDto
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Location { get; set; }
        public bool IsApproved { get; set; }
    }
}