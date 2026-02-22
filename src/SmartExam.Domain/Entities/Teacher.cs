namespace SmartExam.Domain.Entities;

public class Teacher : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Question> Questions { get; set; } = [];
}
