namespace BuildingBlocks.Core.MessageBus
{
    public record RabbitMqSettings
    {
        public string Hostname { get; init; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
    }

    public record ServiceBusSettings
    {
        public string ConnectionString { get; init; }
        public bool EnableSessions { get; init; } = false;
        public string TopicPath { get; init; } = "cap";
    }
}