using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public Guid? ImageId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static UserModel MapFromEntity(User user) => new()
    {
        Id          = user.Id,
        FirstName   = user.FirstName,
        LastName    = user.LastName,
        PhoneNumber = user.PhoneNumber,
        DateOfBirth = user.DateOfBirth,
        RoleId      = user.RoleId,
        RoleName    = user.Role?.Name ?? string.Empty,
        ImageId     = user.ImageId,
        CreatedAt   = user.CreatedAt,
        UpdatedAt   = user.UpdatedAt,
    };
}
