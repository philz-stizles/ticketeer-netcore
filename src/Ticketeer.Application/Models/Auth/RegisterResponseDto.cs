using System.ComponentModel.DataAnnotations;

namespace Ticketeer.Application.Models.Auth
{
    public class RegisterResponseDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
