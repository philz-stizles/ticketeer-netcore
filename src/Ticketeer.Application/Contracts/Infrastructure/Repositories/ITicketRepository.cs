using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Contracts.Infrastructure.Repositories
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetTickets();
        Task<Ticket> GetTicket(string id);
        Task<IEnumerable<Ticket>> GetTicketByName(string name);

        Task<Ticket> CreateTicket(Ticket entity);
        Task<bool> UpdateTicket(Ticket entity);
        Task<bool> DeleteTicket(string id);
    }
}
