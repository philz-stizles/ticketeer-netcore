using AutoMapper;
using Ticketeer.Application.Models;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Mappers
{
    public class TicketProfile : Profile
    {
        public TicketProfile() {
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<TicketCreateDto, Ticket>();
        }
    }
}
