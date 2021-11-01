using System.Collections.Generic;
using Ticketeer.Domain.Entities;
using Ticketeer.Domain.Enums;

namespace Ticketeer.Application.Contracts.Infrastructure.Services
{
    public interface IJwtService
    {
        string GenerateAuthToken(User user, List<RoleType> roles);
        string GenerateEmailConfirmationToken(string id);
        string ValidateJwtToken(string token);
    }
}
