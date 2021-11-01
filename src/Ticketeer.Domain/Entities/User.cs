using System.Collections.Generic;
using Ticketeer.Domain.Enums;

namespace Ticketeer.Domain.Entities
{
    public class User: BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = false;
        public byte[] HashedPassword { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<RoleType> Roles { get; set; }
    }
}
