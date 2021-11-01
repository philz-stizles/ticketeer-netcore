using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Models;

namespace Ticketeer.Application.Contracts.Services
{
    public interface ITransactionService
    {
        Task<List<TicketDto>> GetAsync();

        Task<TicketDto> GetAsync(string id);
        Task RemoveAsync(string id);
    }
}
