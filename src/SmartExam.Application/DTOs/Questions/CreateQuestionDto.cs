namespace SmartExam.Application.DTOs.Questions;

public class CreateQuestionDto
{
    public string Title { get; set; } = string.Empty;
    public string AnswerA { get; set; } = string.Empty;
    public string AnswerB { get; set; } = string.Empty;
    public string AnswerC { get; set; } = string.Empty;
    public string AnswerD { get; set; } = string.Empty;
    public int CorrectAnswer { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public Guid TopicId { get; set; }
    public Guid TeacherId { get; set; }
    public Guid LanguageId { get; set; }
    public Guid? ImageId { get; set; }
}
