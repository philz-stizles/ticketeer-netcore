using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Ticketeer.Domain.Entities
{
    public abstract class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}