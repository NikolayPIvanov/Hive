using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hive.Chat.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        
        public Guid SenderIdentifier { get; set; }

        public string Text { get; set; }

        public DateTime DateTime { get; set; }
    }
    
    
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public string ParticipantOne { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public string ParticipantTwo { get; set; }
        
        public List<Message> Messages { get; set; }
    }
}