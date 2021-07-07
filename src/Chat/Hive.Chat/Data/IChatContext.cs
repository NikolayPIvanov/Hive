using Hive.Chat.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Hive.Chat.Data
{
    public interface IChatContext
    {
        IMongoCollection<UserIdentifier> UserIdentifiers { get; set; }
        
        IMongoCollection<Room> Rooms { get; set; }

    }

    public class ChatDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        
    }
    
    public class ChatContext : IChatContext
    {
        public ChatContext(IOptions<ChatDatabaseSettings> mongoSettings)
        {
            var settings = mongoSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            UserIdentifiers = database.GetCollection<UserIdentifier>("UserIdentifiers");
            Rooms = database.GetCollection<Room>("Rooms");
        }
        public IMongoCollection<UserIdentifier> UserIdentifiers { get; set; }
        public IMongoCollection<Room> Rooms { get; set; }
    }
}