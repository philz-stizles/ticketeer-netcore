using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Models.Orders;

namespace Ticketeer.Application.Contracts.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAsync();

        Task<OrderDto> GetAsync(string id);

        Task<OrderDto> CreateAsync(OrderCreateDto ticketCreateDto);

        Task UpdateAsync(string id, OrderDto ticketDto);

        /*Task RemoveAsync(OrderDto ticketDto);*/

        Task RemoveAsync(string id);
    }
}
