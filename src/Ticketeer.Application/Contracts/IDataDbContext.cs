using MongoDB.Driver;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Contracts
{
    public interface IDataDbContext
    {
        IMongoCollection<Permission> Permissions { get; }
        IMongoCollection<User> Users { get; }
        IMongoCollection<Ticket> Tickets { get; }
        IMongoCollection<Order> Orders { get; }
        IMongoCollection<Transaction> Transactions { get; }
        IMongoCollection<Token> Tokens { get; }
        IMongoCollection<Audit> Audit { get; }
    }
}
