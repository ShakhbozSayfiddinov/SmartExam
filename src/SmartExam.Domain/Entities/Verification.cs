namespace SmartExam.Entities;

public class Verification : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public Guid Guid { get; set; }
}
