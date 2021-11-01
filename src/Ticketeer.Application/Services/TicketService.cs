using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Infrastructure.Repositories;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ticketRepository = ticketRepository;
        }

        public async Task<List<TicketDto>> GetAsync()
        {
            var tickets = await _ticketRepository.GetTickets();
            return _mapper.Map<List<TicketDto>>(tickets);
        }

        public async Task<TicketDto> GetAsync(string id) =>
             _mapper.Map<TicketDto>((await _ticketRepository.GetTicket(id)));

        public async Task<TicketDto> CreateAsync(TicketCreateDto ticketCreateDto)
        {
            var newTicket = _mapper.Map<Ticket>(ticketCreateDto);
            var createdTicket = await _ticketRepository.CreateTicket(newTicket);
            return _mapper.Map<TicketDto>(createdTicket);
        }

        public async Task UpdateAsync(string id, TicketDto ticketDto) =>
            await _ticketRepository.UpdateTicket(_mapper.Map<Ticket>(ticketDto));

        /*public async Task RemoveAsync(TicketDto ticketDto) =>
            await _ticketRepository.DeleteTicket(_mapper.Map<Ticket>(ticketDto));*/

        public async Task RemoveAsync(string id) =>
            await _ticketRepository.DeleteTicket(id);
    }
}
