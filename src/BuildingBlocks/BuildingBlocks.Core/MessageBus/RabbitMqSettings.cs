namespace BuildingBlocks.Core.MessageBus
{
    public record RabbitMqSettings(string Hostname, int Port, string UserName, string Password, string VirtualHost);
}