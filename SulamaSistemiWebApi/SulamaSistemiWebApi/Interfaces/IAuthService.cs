using SulamaSistemiWebApi.Models;
using SulamaSistemiWebApi.DTOs;

namespace SulamaSistemiWebApi.Interfaces;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<User?> GetUserByIdAsync(int id);
}