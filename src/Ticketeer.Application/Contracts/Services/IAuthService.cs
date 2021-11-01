using System.Threading.Tasks;
using Ticketeer.Application.Models.Auth;

namespace Ticketeer.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto);

        Task<LoggedInUserDto> ConfirmEmailAsync(string token);
        Task<LoggedInUserDto> LoginAsync(LoginDto loginDto);
    }
}
