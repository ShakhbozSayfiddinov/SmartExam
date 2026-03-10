using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class ScienceModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ScienceId { get; set; }
    public string ParentName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static ScienceModel MapFromEntity(Science science) => new()
    {
        Id          = science.Id,
        Name        = science.Name,
        Description = science.Description,
        ScienceId   = science.ScienceId,
        ParentName  = science.ParentScience?.Name,
        CreatedAt   = science.CreatedAt,
        UpdatedAt   = science.UpdatedAt,
    };
}
