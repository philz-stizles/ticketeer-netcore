using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Ticketeer.Application.Contracts;
using Ticketeer.Application.Contracts.Infrastructure.Repositories;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Infrastructure.Persistence.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IDataDbContext _context;

        public TicketRepository(IDataDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Ticket> CreateTicket(Ticket entity)
        {
            await _context.Tickets.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteTicket(string id)
        {
            var result = await _context.Tickets.DeleteOneAsync(t => t.Id == id);

            return result.DeletedCount > 0;
        }

        public Task<Ticket> GetTicket(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetTicketByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _context
                            .Tickets
                            .Find(p => true)
                            .ToListAsync();
        }

        public Task<bool> UpdateTicket(Ticket entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
