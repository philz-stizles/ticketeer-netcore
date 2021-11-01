using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models.Orders;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Mappers.Resolvers
{
    public class OrderTicketsResolver : IValueResolver<OrderCreateDto, Order, List<string>>
    {
        private readonly ITicketService _ticketService;

        public OrderTicketsResolver(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public List<string> Resolve(OrderCreateDto source, Order destination, List<string> destMember, ResolutionContext context)
        {
            var tickets = new List<string>();
            foreach(var ticket in source.Tickets)
            {
                if(Task.FromResult(_ticketService.GetAsync(ticket)) != null)
                {
                    tickets.Add(ticket);
                }
            }

            return tickets;
        }
    }
}
