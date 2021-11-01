using AutoMapper;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models.Auth;
using Ticketeer.Application.Contracts.Infrastructure.Repositories;
using Ticketeer.Application.Exceptions;
using System.Net;
using Ticketeer.Domain.Entities;
using Ticketeer.Domain.Enums;
using Ticketeer.Application.Contracts.Infrastructure.Services;
using Hangfire;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Hosting;
using Ticketeer.Application.Settings;
using Microsoft.Extensions.Options;
using System.IO;
using System.Collections.Generic;

namespace Ticketeer.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly AppSettings _appSettings;

        public AuthService(IUserRepository userRepository, IPasswordService passwordService,
            IJwtService jwtService, IMapper mapper, IWebHostEnvironment hostingEnv, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _mapper = mapper;
            _hostingEnv = hostingEnv;
            _appSettings = appSettings.Value;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if user exists
            var exists = await _userRepository.Exists(registerDto.Email, registerDto.Username);
            if(exists)
            {
                throw new RestException(HttpStatusCode.BadRequest, "User already exists");
            }

            // Map user dto to user entity.
            var newUser = _mapper.Map<User>(registerDto);

            // Assign user roles.
            newUser.Roles = new List<RoleType>();
            if(registerDto.UserType == RoleType.Vendor.ToString())
            {
                newUser.Roles.Add(RoleType.Vendor);
            }

            if (registerDto.UserType == RoleType.User.ToString())
            {
                newUser.Roles.Add(RoleType.User);
            }

            // Hash user password.
            byte[] passwordHash, passwordSalt;
            _passwordService.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);
            newUser.HashedPassword = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            // Create User.
            var createdUser = await _userRepository.CreateUser(newUser);

            // Send email verification message to user email.
            var token = _jwtService.GenerateEmailConfirmationToken(createdUser.Id);
            var confirmationLink = $"{_appSettings.WebUri}/confirm-email.html?id={createdUser.Id}&token={token}";
            string projectRootPath = _hostingEnv.ContentRootPath;
            string notificationEmailPath = Path.Combine(projectRootPath, "EmailTemplates/EmailConfirmation.html");
            string htmlContent = File.ReadAllText(notificationEmailPath);
            htmlContent = htmlContent.Replace("##EMAIL##", newUser.Email);
            htmlContent = htmlContent.Replace("##WEBURI##", _appSettings.WebUri);
            htmlContent = htmlContent.Replace("##VERIFICATIONLINK##", HtmlEncoder.Default.Encode(confirmationLink));
            BackgroundJob.Enqueue<IEmailService>(x => x.SendHTMLEmailAsync("Ticketeer - Email Confirmation",
                newUser.Email, htmlContent));

            // Return response.
            return new RegisterResponseDto {
                Email = newUser.Email,
                Username = newUser.Username
            };
        }

        public async Task<LoggedInUserDto> ConfirmEmailAsync(string token)
        {
            // Validate email confirmation token.
            var userId = _jwtService.ValidateJwtToken(token);
            if (userId == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, "Invalid credentials");
            }

            // Check if user exists
            var existingUser = await _userRepository.GetUser(userId);
            if (existingUser == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, "Invalid credentials");
            }

            existingUser.IsActive = true;
            var updated = await _userRepository.UpdateUser(existingUser);
            if (!updated)
            {
                throw new RestException(HttpStatusCode.BadRequest, 
                    "Email confirmation unsuccessful, please try again later");
            }

            return new LoggedInUserDto
            {
                Email = existingUser.Email,
                Token = token
            };
        }

        public async Task<LoggedInUserDto> LoginAsync(LoginDto loginDto)
        {
            // Check if user exists
            var existingUser = await _userRepository.GetUserByLogin(loginDto.Email);
            if (existingUser == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, "Invalid email/password");
            }

            // Hash user password.
            var valid = _passwordService.VerifyPasswordHash(loginDto.Password, existingUser.HashedPassword, 
                existingUser.PasswordSalt);
            if (!valid)
            {
                throw new RestException(HttpStatusCode.BadRequest, "Invalid email/password");
            }

            // Check email verification.
            if (!existingUser.IsActive)
            {
                throw new RestException(HttpStatusCode.BadRequest, "Pease verify your email");
            }

            // Generate authentication token
            var token = _jwtService.GenerateAuthToken(existingUser, existingUser.Roles);

            return new LoggedInUserDto { 
                Email = existingUser.Email,
                Token = token
            };
        }
    }
}
