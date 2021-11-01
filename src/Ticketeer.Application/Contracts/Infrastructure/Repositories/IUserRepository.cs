using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Contracts.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByLogin(string login);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(string id);
        Task<bool> Exists(string email, string username);
        Task<IEnumerable<User>> GetUserByName(string name);

        Task<User> CreateUser(User entity);
        Task<bool> UpdateUser(User entity);
        Task<bool> DeleteUser(string id);
    }
}
