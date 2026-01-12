namespace SmartExam.Models.Questions;

public class QuestionResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AnswerA { get; set; } = string.Empty;
    public string AnswerB { get; set; } = string.Empty;
    public string AnswerC { get; set; } = string.Empty;
    public string AnswerD { get; set; } = string.Empty;
    public char CorrectAnswer { get; set; }
    public string Explanation { get; set; }
    public int TopicId { get; set; }
    public int? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
