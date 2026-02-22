namespace SmartExam.Domain.Entities;

public class Student : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public bool IsDeleted { get; set; }
}
