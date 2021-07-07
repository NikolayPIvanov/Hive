namespace BuildingBlocks.Core.Email
{
    public record AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridSenderEmail { get; set; }
        public string SendGridKey { get; set; }
    }
}