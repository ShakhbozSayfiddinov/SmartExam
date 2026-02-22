namespace SmartExam.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public Guid? ImageId { get; set; }
    public Attachment Image { get; set; }

    public bool IsDeleted { get; set; }

    public Teacher Teacher { get; set; }
    public Student Student { get; set; }
}
