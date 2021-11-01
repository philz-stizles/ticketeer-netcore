using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models;
using Ticketeer.Application.Models.Orders;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Mappers.Resolvers
{
    public class OrderDtoTicketsResolver : IValueResolver<Order, OrderDto, List<TicketDto>>
    {
        private readonly ITicketService _ticketService;

        public OrderDtoTicketsResolver(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public List<TicketDto> Resolve(Order source, OrderDto destination, List<TicketDto> destMember, ResolutionContext context)
        {
            var tickets = new List<TicketDto>();
            foreach(var ticket in source.Tickets)
            {
                var existingTicket = _ticketService.GetAsync(ticket).Result;
                if (existingTicket != null)
                {
                    tickets.Add(new TicketDto {
                        Name = existingTicket.Name,
                        Description = existingTicket.Description,
                        Price = existingTicket.Price
                    });
                }
            }

            return tickets;
        }
    }
}
