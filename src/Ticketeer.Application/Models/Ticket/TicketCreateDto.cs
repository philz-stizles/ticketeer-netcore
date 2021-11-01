using System.ComponentModel.DataAnnotations;

namespace Ticketeer.Application.Models
{
    public class TicketCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
