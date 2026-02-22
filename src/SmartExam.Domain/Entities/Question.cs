namespace SmartExam.Domain.Entities;

public class Question : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string AnswerA { get; set; } = string.Empty;
    public string AnswerB { get; set; } = string.Empty;
    public string AnswerC { get; set; } = string.Empty;
    public string AnswerD { get; set; } = string.Empty;
    public int CorrectAnswer { get; set; }
    public string Explation { get; set; } = string.Empty;

    public Guid TopicId { get; set; }
    public Topic Topic { get; set; }

    public bool IsDeleted { get; set; }

    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public Guid? ImageId { get; set; }
    public Attachment Image { get; set; }
}
