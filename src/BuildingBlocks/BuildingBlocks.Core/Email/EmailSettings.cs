namespace BuildingBlocks.Core.Email
{
    public record EmailSettings(string Host, int Port, string FromEmail, string Username, string Password);
}