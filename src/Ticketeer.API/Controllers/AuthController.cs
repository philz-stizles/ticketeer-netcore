using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models.Auth;

namespace Ticketeer.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseDto))]
        public async Task<ActionResult<RegisterResponseDto>> RegisterWithEmailVerification(
            RegisterDto registerDto)
        {
            var tickets = await _authService.RegisterAsync(registerDto);
            return Ok(tickets);
        }

        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoggedInUserDto))]
        public async Task<ActionResult<LoggedInUserDto>> ConfirmEmail([FromQuery] string token)
        {
            var loggedInUserDto = await _authService.ConfirmEmailAsync(token);
            return Ok(loggedInUserDto);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoggedInUserDto))]
        public async Task<ActionResult<LoggedInUserDto>> Login(LoginDto loginDto)
        {
            var loggedInUserDto = await _authService.LoginAsync(loginDto);
            return Ok(loggedInUserDto);
        }
    }
}
