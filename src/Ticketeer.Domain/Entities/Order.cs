using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using Ticketeer.Domain.Enums;

namespace Ticketeer.Domain.Entities
{
    public class Order: BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Tickets { get; set; }
        [BsonIgnore]
        public List<Ticket> TicketList { get; set; }
        public TimeSpan ExpiresAt { get; set; }
        public OrderStatus Status { get; set; }
        public User User { get; set; }
        public decimal TotalAmount { get; set; }

        public int TotalItems
        {
            get { return TicketList.Count; }
        }
    }
}
