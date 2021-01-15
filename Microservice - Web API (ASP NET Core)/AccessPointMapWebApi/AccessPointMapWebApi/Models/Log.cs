using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AccessPointMapWebApi.Models
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
