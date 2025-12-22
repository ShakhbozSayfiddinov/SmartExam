namespace SmartExam.Entities;

public class Question : BaseEntity
{
    public string Text { get; set; } = string.Empty;

    public string? Answer { get; set; }

    public int TopicId { get; set; }

    public Topic? Topic { get; set; }
    public bool IsDeleted { get; set; }

    public int? CreatedByUserId { get; set; }

    public User? CreatedBy { get; set; }
}
