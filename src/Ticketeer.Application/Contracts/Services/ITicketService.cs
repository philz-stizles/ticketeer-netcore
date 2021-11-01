using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Models;

namespace Ticketeer.Application.Contracts.Services
{
    public interface ITicketService
    {
        Task<List<TicketDto>> GetAsync();

        Task<TicketDto> GetAsync(string id);

        Task<TicketDto> CreateAsync(TicketCreateDto ticketCreateDto);

        Task UpdateAsync(string id, TicketDto ticketDto);

        /*Task RemoveAsync(TicketDto ticketDto);*/

        Task RemoveAsync(string id);
    }
}
