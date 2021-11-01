using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Ticketeer.Application.Contracts;
using Ticketeer.Application.Contracts.Infrastructure.Repositories;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDataDbContext _context;

        public OrderRepository(IDataDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Order> CreateOrder(Order entity)
        {
            await _context.Orders.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteOrder(string id)
        {
            var result = await _context.Orders.DeleteOneAsync(t => t.Id == id);

            return result.DeletedCount > 0;
        }

        public Task<Order> GetOrder(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrderByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context
                            .Orders
                            .Find(p => true )
                            .ToListAsync();
        }

        public Task<bool> UpdateOrder(Order entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
