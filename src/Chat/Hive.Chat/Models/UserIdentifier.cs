using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hive.Chat.Models
{
    public class UserIdentifier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string UniqueIdentifier { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }
    }
}