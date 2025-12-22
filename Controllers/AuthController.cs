using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartExam.Exceptions;
using SmartExam.Models.Auth;
using SmartExam.Services.Interfaces;

namespace SmartExam.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

[AllowAnonymous]
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterRequest request)
{
        try
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }
        catch (SmartExamException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

[AllowAnonymous]
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (SmartExamException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }
}
