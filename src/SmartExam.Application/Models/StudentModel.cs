using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class StudentModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static StudentModel MapFromEntity(Student student) => new()
    {
        Id          = student.Id,
        UserId      = student.UserId,
        FirstName   = student.User?.FirstName ?? string.Empty,
        LastName    = student.User?.LastName ?? string.Empty,
        PhoneNumber = student.User?.PhoneNumber ?? string.Empty,
        CreatedAt   = student.CreatedAt,
        UpdatedAt   = student.UpdatedAt,
    };
}
