using System;
using System.Collections.Generic;
using Ticketeer.Application.Models.User;
using Ticketeer.Domain.Enums;

namespace Ticketeer.Application.Models.Orders
{
    public class OrderDto
    {
        public string Id { get; set; }
        public List<TicketDto> Tickets { get; set; }
        public TimeSpan ExpiresAt { get; set; }
        public OrderStatus Status { get; set; }
        public UserDto createdBy { get; set; }
        public decimal TotalAmount { get; set; }

        public int TotalItems { get; set; }
    }
}
