using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts;
using Ticketeer.Application.Contracts.Infrastructure.Repositories;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataDbContext _context;

        public UserRepository(IDataDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> CreateUser(User entity)
        {
            await _context.Users.InsertOneAsync(entity);
            return entity;
        }

        public Task<bool> DeleteUser(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Exists(string email, string username)
        {
            var existingUser = await _context.Users.Find(u => u.Email == email || u.Username == username)
                .FirstOrDefaultAsync();

            return existingUser != null;
        }

        public async Task<User> GetUser(string id)
        {
            return await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await _context.Users.Find(u => u.Username == login || u.Email == login).FirstOrDefaultAsync();
        }

        public Task<IEnumerable<User>> GetUserByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context
                            .Users
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<bool> UpdateUser(User entity)
        {
            await _context
                            .Users.ReplaceOneAsync(u => u.Id == entity.Id, entity);

            return true;
        }
    }
}
