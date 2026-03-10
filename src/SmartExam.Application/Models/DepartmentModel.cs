using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class DepartmentModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid ScienceId { get; set; }
    public string ScienceName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static DepartmentModel MapFromEntity(Department department) => new()
    {
        Id          = department.Id,
        Name        = department.Name,
        Description = department.Description,
        ScienceId   = department.ScienceId,
        ScienceName = department.Science?.Name ?? string.Empty,
        CreatedAt   = department.CreatedAt,
        UpdatedAt   = department.UpdatedAt,
    };
}
