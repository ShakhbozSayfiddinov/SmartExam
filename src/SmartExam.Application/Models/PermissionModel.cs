using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class PermissionModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static PermissionModel MapFromEntity(Permission permission) => new()
    {
        Id          = permission.Id,
        Name        = permission.Name,
        Description = permission.Description,
        CreatedAt   = permission.CreatedAt,
        UpdatedAt   = permission.UpdatedAt,
    };
}
