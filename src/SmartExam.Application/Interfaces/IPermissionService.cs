using SmartExam.Application.DTOs.Permissions;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IPermissionService
{
    Task<List<PermissionModel>> GetAllAsync();
    Task<PermissionModel> GetByIdAsync(Guid id);
    Task<PermissionModel> CreateAsync(CreatePermissionDto dto);
    Task<PermissionModel> UpdateAsync(Guid id, UpdatePermissionDto dto);
    Task DeleteAsync(Guid id);
}
