using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Users;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class UsersController(IUserService userService, IFileStorageService fileStorage) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAllAsync();
        return Success(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await userService.GetByIdAsync(id);
        return user is null ? NotFound() : Success(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateUserDto dto, IFormFile image = null)
    {
        string imageUrl = image is not null ? await fileStorage.SaveAsync(image) : null;
        var user = await userService.CreateAsync(dto, imageUrl);
        return Created(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUserDto dto, IFormFile image = null)
    {
        string imageUrl = image is not null ? await fileStorage.SaveAsync(image) : null;
        var user = await userService.UpdateAsync(id, dto, imageUrl);
        return Success(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await userService.DeleteAsync(id);
        return NoContent();
    }
}
