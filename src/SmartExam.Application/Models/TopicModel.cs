using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class TopicModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static TopicModel MapFromEntity(Topic topic) => new()
    {
        Id             = topic.Id,
        Name           = topic.Name,
        Description    = topic.Description,
        DepartmentId   = topic.DepartmentId,
        DepartmentName = topic.Department?.Name ?? string.Empty,
        CreatedAt      = topic.CreatedAt,
        UpdatedAt      = topic.UpdatedAt,
    };
}
