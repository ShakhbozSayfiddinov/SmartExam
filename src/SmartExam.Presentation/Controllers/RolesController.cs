using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Roles;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class RolesController(IRoleService roleService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await roleService.GetAllAsync();
        return Success(roles);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var role = await roleService.GetByIdAsync(id);
        return role is null ? NotFound() : Success(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
    {
        var role = await roleService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = role.Id }, role);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto dto)
    {
        var role = await roleService.UpdateAsync(id, dto);
        return Success(role);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await roleService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}/permissions")]
    public async Task<IActionResult> GetPermissions(Guid id)
    {
        var permissions = await roleService.GetPermissionsAsync(id);
        return Success(permissions);
    }

    [HttpPost("{id:guid}/permissions/{permissionId:guid}")]
    public async Task<IActionResult> AssignPermission(Guid id, Guid permissionId)
    {
        await roleService.AssignPermissionAsync(id, permissionId);
        return NoContent();
    }

    [HttpDelete("{id:guid}/permissions/{permissionId:guid}")]
    public async Task<IActionResult> RemovePermission(Guid id, Guid permissionId)
    {
        await roleService.RemovePermissionAsync(id, permissionId);
        return NoContent();
    }
}
