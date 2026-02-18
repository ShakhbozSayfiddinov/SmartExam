namespace SmartExam.Domain.Entities;

public class Question : BaseEntity
{
    public string Title { get; set; }

    public string AnswerA { get; set; }
    public string AnswerB { get; set; }
    public string AnswerC { get; set; }
    public string AnswerD { get; set; }
    public char CorrectAnswer { get; set; }
    public string  Explation { get; set; }

    public int TopicId { get; set; }

    public Topic Topic { get; set; }
    public bool IsDeleted { get; set; }

    public int? CreatedByUserId { get; set; }

    public User CreatedBy { get; set; }
}
