using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Roles;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class RoleService(AppDbContext context) : IRoleService
{
    public async Task<List<RoleModel>> GetAllAsync()
    {
        var roles = await context.Roles
            .Where(r => !r.IsDeleted)
            .ToListAsync();

        return roles.Select(RoleModel.MapFromEntity).ToList();
    }

    public async Task<RoleModel> GetByIdAsync(Guid id)
    {
        var role = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        return role is null ? null : RoleModel.MapFromEntity(role);
    }

    public async Task<RoleModel> CreateAsync(CreateRoleDto dto)
    {
        var role = new Role
        {
            Name        = dto.Name,
            Description = dto.Description,
            IsActive    = dto.IsActive,
        };

        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();

        return RoleModel.MapFromEntity(role);
    }

    public async Task<RoleModel> UpdateAsync(Guid id, UpdateRoleDto dto)
    {
        var role = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted)
            ?? throw new SmartExamException(404, "error_role_not_found");

        role.Name        = dto.Name;
        role.Description = dto.Description;
        role.IsActive    = dto.IsActive;
        role.UpdatedAt   = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return RoleModel.MapFromEntity(role);
    }

    public async Task DeleteAsync(Guid id)
    {
        var role = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted)
            ?? throw new SmartExamException(404, "error_role_not_found");

        role.IsDeleted = true;
        role.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task AssignPermissionAsync(Guid roleId, Guid permissionId)
    {
        var roleExists = await context.Roles.AnyAsync(r => r.Id == roleId && !r.IsDeleted);
        if (!roleExists)
            throw new SmartExamException(404, "error_role_not_found");

        var permissionExists = await context.Permissions.AnyAsync(p => p.Id == permissionId && !p.IsDeleted);
        if (!permissionExists)
            throw new SmartExamException(404, "error_permission_not_found");

        var alreadyAssigned = await context.RolePermissions
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (alreadyAssigned)
            throw new SmartExamException(409, "error_permission_already_assigned");

        await context.RolePermissions.AddAsync(new RolePermission
        {
            RoleId       = roleId,
            PermissionId = permissionId,
        });

        await context.SaveChangesAsync();
    }

    public async Task RemovePermissionAsync(Guid roleId, Guid permissionId)
    {
        var rolePermission = await context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId)
            ?? throw new SmartExamException(404, "error_role_permission_not_found");

        context.RolePermissions.Remove(rolePermission);
        await context.SaveChangesAsync();
    }

    public async Task<List<PermissionModel>> GetPermissionsAsync(Guid roleId)
    {
        var roleExists = await context.Roles.AnyAsync(r => r.Id == roleId && !r.IsDeleted);
        if (!roleExists)
            throw new SmartExamException(404, "error_role_not_found");

        var permissions = await context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Include(rp => rp.Permission)
            .Where(rp => !rp.Permission.IsDeleted)
            .Select(rp => rp.Permission)
            .ToListAsync();

        return permissions.Select(PermissionModel.MapFromEntity).ToList();
    }
}
