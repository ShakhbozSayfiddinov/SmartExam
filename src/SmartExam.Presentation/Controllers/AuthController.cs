using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Auth;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class AuthController(IAuthService authService) : AppController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await authService.RegisterAsync(dto);
        return Success(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await authService.LoginAsync(dto);
        return Success(result);
    }
}
