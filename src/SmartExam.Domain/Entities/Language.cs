namespace SmartExam.Domain.Entities;

public class Language : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public ICollection<Question> Questions { get; set; } = [];
}
