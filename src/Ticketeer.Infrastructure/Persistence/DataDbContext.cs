using MongoDB.Driver;
using Ticketeer.Application.Contracts;
using Ticketeer.Application.Contracts.Settings;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Infrastructure.Persistence
{
    public class DataDbContext : IDataDbContext
    {
        public DataDbContext(IMonogDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Permissions = database.GetCollection<Permission>("permissions");
            Users = database.GetCollection<User>("users");
            Tickets = database.GetCollection<Ticket>("tickets");
            Orders = database.GetCollection<Order>("orders");
            Transactions = database.GetCollection<Transaction>("transactions");
            Tokens = database.GetCollection<Token>("tokens");
            Audit = database.GetCollection<Audit>("audit");
            // CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Permission> Permissions { get; }

        public IMongoCollection<User> Users { get; }

        public IMongoCollection<Ticket> Tickets { get; }

        public IMongoCollection<Order> Orders { get; }

        public IMongoCollection<Transaction> Transactions { get; }

        public IMongoCollection<Token> Tokens { get; }

        public IMongoCollection<Audit> Audit { get; }
    }
}
