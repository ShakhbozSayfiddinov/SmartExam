namespace SmartExam.Application.Models.Questions;

public class QuestionUpdateRequest
{
    public string Title { get; set; }
    public string AnswerA { get; set; }
    public string AnswerB { get; set; }
    public string AnswerC { get; set; }
    public string AnswerD { get; set; }
    public string CorrectAnswer { get; set; }
    public string Explanation { get; set; }
    public int? TopicId { get; set; }
}
