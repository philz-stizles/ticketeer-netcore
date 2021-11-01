using AutoMapper;
using Ticketeer.Application.Mappers.Resolvers;
using Ticketeer.Application.Models.Orders;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile() {
            CreateMap<Order, OrderDto>()
                .ForMember(o => o.Tickets, opt => opt.MapFrom<OrderDtoTicketsResolver>());
            CreateMap<OrderCreateDto, Order>()
                .ForMember(o => o.Tickets, opt => opt.MapFrom<OrderTicketsResolver>());
        }
    }
}
