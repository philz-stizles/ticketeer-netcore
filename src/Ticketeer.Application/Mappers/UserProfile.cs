using AutoMapper;
using Ticketeer.Application.Models.Auth;
using Ticketeer.Application.Models.User;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<RegisterDto, User>();
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
