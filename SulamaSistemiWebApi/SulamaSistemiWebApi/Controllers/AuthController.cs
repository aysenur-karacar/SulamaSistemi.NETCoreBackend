using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SulamaSistemiWebApi.Models;
using SulamaSistemiWebApi.DTOs;
using SulamaSistemiWebApi.Services;
using SulamaSistemiWebApi.Interfaces;

namespace SulamaSistemiWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _authService.LoginAsync(request);
        
        if (response == null)
        {
            return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre" });
        }

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _authService.RegisterAsync(request);
        
        if (response == null)
        {
            return BadRequest(new { message = "Kullanıcı adı veya email zaten kullanımda" });
        }

        return Ok(response);
    }
} 