using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Infrastructure.Repositories;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models.Auth;
using Ticketeer.Application.Models.User;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAsync()
        {
            var users = await _userRepository.GetUsers();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetAsync(string id) =>
             _mapper.Map<UserDto>((await _userRepository.GetUser(id)));

        public async Task<UserDto> CreateAsync(RegisterDto registerDto)
        {
            var newUser = _mapper.Map<User>(registerDto);
            await _userRepository.CreateUser(newUser);
            return _mapper.Map<UserDto>(newUser);
        }

        public async Task UpdateAsync(string id, UserDto userDto) =>
            await _userRepository.UpdateUser(_mapper.Map<User>(userDto));

        /*public async Task RemoveAsync(UserDto userDto) =>
            await _userRepository.DeleteUser(_mapper.Map<User>(userDto));*/

        public async Task RemoveAsync(string id) =>
            await _userRepository.DeleteUser(id);
    }
}
