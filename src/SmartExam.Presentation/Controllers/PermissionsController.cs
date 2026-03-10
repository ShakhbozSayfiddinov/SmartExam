using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Permissions;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class PermissionsController(IPermissionService permissionService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var permissions = await permissionService.GetAllAsync();
        return Success(permissions);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var permission = await permissionService.GetByIdAsync(id);
        return permission is null ? NotFound() : Success(permission);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePermissionDto dto)
    {
        var permission = await permissionService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = permission.Id }, permission);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePermissionDto dto)
    {
        var permission = await permissionService.UpdateAsync(id, dto);
        return Success(permission);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await permissionService.DeleteAsync(id);
        return NoContent();
    }
}
