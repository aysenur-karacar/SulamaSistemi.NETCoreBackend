using System.Security.Claims;
using SulamaSistemiWebApi.Models;

namespace SulamaSistemiWebApi.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
}
