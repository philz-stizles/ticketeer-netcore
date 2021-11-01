using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Ticketeer.Application.Contracts.Services;

namespace Ticketeer.Infrastructure.Services.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            try
            {
                if (_httpContextAccessor.HttpContext == null)
                {
                    return null;
                }

                return _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public string GetCurrentEmail()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return null;
            }

            return _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }

        public string GetCurrentUserName()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return null;
            }

            return _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }

        public string GetCurrentUserIp()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return null;
            }

            return Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
        }

        public string GetCurrentUserToken()
        {
            return _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        }

        public List<string> GetCurrentUserRoles()
        {
            try
            {
                var roles = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role);
                return (roles == null) ? new List<string> { "" } : roles.Select(x => x.Value).ToList();
            }
            catch (Exception)
            {
                return new List<string> { "" };
            }
        }
    }
}
