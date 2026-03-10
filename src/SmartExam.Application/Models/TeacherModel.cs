using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class TeacherModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static TeacherModel MapFromEntity(Teacher teacher) => new()
    {
        Id          = teacher.Id,
        UserId      = teacher.UserId,
        FirstName   = teacher.User?.FirstName ?? string.Empty,
        LastName    = teacher.User?.LastName ?? string.Empty,
        PhoneNumber = teacher.User?.PhoneNumber ?? string.Empty,
        CreatedAt   = teacher.CreatedAt,
        UpdatedAt   = teacher.UpdatedAt,
    };
}
