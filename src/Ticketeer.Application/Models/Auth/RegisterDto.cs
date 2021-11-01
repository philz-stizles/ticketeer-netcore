using System.ComponentModel.DataAnnotations;
using Ticketeer.Domain.Enums;

namespace Ticketeer.Application.Models.Auth
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public RoleType UserType { get; set; }
    }
}
