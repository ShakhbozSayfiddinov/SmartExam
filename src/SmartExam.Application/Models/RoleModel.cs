using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class RoleModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static RoleModel MapFromEntity(Role role) => new()
    {
        Id          = role.Id,
        Name        = role.Name,
        Description = role.Description,
        IsActive    = role.IsActive,
        CreatedAt   = role.CreatedAt,
        UpdatedAt   = role.UpdatedAt,
    };
}
