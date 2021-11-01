using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ticketeer.Domain.Entities
{
    public class Ticket: BaseEntity
    {

        [BsonElement("name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public User Owner { get; set; }
    }
}
