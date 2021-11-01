using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Models.Auth;
using Ticketeer.Application.Models.User;

namespace Ticketeer.Application.Contracts.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAsync();

        Task<UserDto> GetAsync(string id);

        Task<UserDto> CreateAsync(RegisterDto ticketCreateDto);

        Task UpdateAsync(string id, UserDto ticketDto);

        /*Task RemoveAsync(UserDto ticketDto);*/

        Task RemoveAsync(string id);
    }
}
