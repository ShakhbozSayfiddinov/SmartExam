using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Permissions;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class PermissionService(AppDbContext context) : IPermissionService
{
    public async Task<List<PermissionModel>> GetAllAsync()
    {
        var permissions = await context.Permissions
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return permissions.Select(PermissionModel.MapFromEntity).ToList();
    }

    public async Task<PermissionModel> GetByIdAsync(Guid id)
    {
        var permission = await context.Permissions
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        return permission is null ? null : PermissionModel.MapFromEntity(permission);
    }

    public async Task<PermissionModel> CreateAsync(CreatePermissionDto dto)
    {
        var permission = new Permission
        {
            Name        = dto.Name,
            Description = dto.Description,
        };

        await context.Permissions.AddAsync(permission);
        await context.SaveChangesAsync();

        return PermissionModel.MapFromEntity(permission);
    }

    public async Task<PermissionModel> UpdateAsync(Guid id, UpdatePermissionDto dto)
    {
        var permission = await context.Permissions
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new SmartExamException(404, "error_permission_not_found");

        permission.Name        = dto.Name;
        permission.Description = dto.Description;
        permission.UpdatedAt   = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return PermissionModel.MapFromEntity(permission);
    }

    public async Task DeleteAsync(Guid id)
    {
        var permission = await context.Permissions
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new SmartExamException(404, "error_permission_not_found");

        permission.IsDeleted = true;
        permission.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
