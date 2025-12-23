namespace SmartExam.Models.Questions;

public class QuestionCreateRequest
{
    public string Title { get; set; } = string.Empty;
    public string AnswerA { get; set; } = string.Empty;
    public string AnswerB { get; set; } = string.Empty;
    public string AnswerC { get; set; } = string.Empty;
    public string AnswerD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public int TopicId { get; set; }
}
