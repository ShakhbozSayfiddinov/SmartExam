using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartExam.Exceptions;
using SmartExam.Models.Users;
using SmartExam.Services.Interfaces;

namespace SmartExam.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var actorUserId = GetActorId();
            var users = await _userService.GetAllAsync(actorUserId);
            return Ok(users);
        }
        catch (SmartExamException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var actorUserId = GetActorId();
            var user = await _userService.GetByIdAsync(id, actorUserId);
            return Ok(user);
        }
        catch (SmartExamException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
    {
        try
        {
            var actorUserId = GetActorId();
            var user = await _userService.CreateAsync(request, actorUserId);
            return Ok(user);
        }
        catch (SmartExamException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateByAdmin(int id, [FromBody] UserUpdateRequest request)
    {
        try
        {
            EnsureAdmin();
            var actorUserId = GetActorId();
            var user = await _userService.UpdateAsAdminAsync(id, request, actorUserId);
            return Ok(user);
        }
        catch (SmartExamException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UserUpdateRequest request)
    {
        try
        {
            var actorUserId = GetActorId();
            var user = await _userService.UpdateSelfAsync(request, actorUserId);
            return Ok(user);
        }
        catch (SmartExamException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteByAdmin(int id)
    {
        try
        {
            EnsureAdmin();
            var actorUserId = GetActorId();
            await _userService.DeleteAsAdminAsync(id, actorUserId);
            return NoContent();
        }
        catch (SmartExamException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe()
    {
        try
        {
            var actorUserId = GetActorId();
            await _userService.DeleteSelfAsync(actorUserId);
            return NoContent();
        }
        catch (SmartExamException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

    private int GetActorId()
    {
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(idValue) || !int.TryParse(idValue, out var userId))
        {
            throw new SmartExamException(StatusCodes.Status401Unauthorized, "Invalid or missing token.");
        }
        return userId;
    }

    private bool IsAdmin() => User.IsInRole("Admin");

    private void EnsureAdmin()
    {
        if (!IsAdmin())
            throw new SmartExamException(StatusCodes.Status403Forbidden, "Admin privileges required.");
    }
}
