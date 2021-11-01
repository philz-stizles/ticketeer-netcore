using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Contracts.Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(string id);
        Task<IEnumerable<Order>> GetOrderByName(string name);

        Task<Order> CreateOrder(Order entity);
        Task<bool> UpdateOrder(Order entity);
        Task<bool> DeleteOrder(string id);
    }
}
