using System;

namespace Ticketeer.Domain.Entities
{
    public class Token: BaseEntity
    {
        public string token { get; set; }
        public DateTimeOffset ExpiresIn { get; set; }
        public User User { get; set; }
    }
}
