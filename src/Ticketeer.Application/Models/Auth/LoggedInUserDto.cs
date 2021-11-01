using System.ComponentModel.DataAnnotations;

namespace Ticketeer.Application.Models.Auth
{
    public class LoggedInUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
