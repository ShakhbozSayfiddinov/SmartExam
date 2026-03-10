using SmartExam.Application.DTOs.Roles;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IRoleService
{
    Task<List<RoleModel>> GetAllAsync();
    Task<RoleModel> GetByIdAsync(Guid id);
    Task<RoleModel> CreateAsync(CreateRoleDto dto);
    Task<RoleModel> UpdateAsync(Guid id, UpdateRoleDto dto);
    Task DeleteAsync(Guid id);

    Task AssignPermissionAsync(Guid roleId, Guid permissionId);
    Task RemovePermissionAsync(Guid roleId, Guid permissionId);
    Task<List<PermissionModel>> GetPermissionsAsync(Guid roleId);
}
