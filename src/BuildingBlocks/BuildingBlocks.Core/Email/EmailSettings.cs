namespace BuildingBlocks.Core.Email
{
    public record AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridSenderEmail { get; set; }
        public string SendGridKey { get; set; }
    }
    
    public record EmailSettings
    {
        public string Host { get; init; }
        public int Port { get; init; }
        public string FromEmail { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
    }
}