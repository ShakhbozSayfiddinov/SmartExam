using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class QuestionModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AnswerA { get; set; } = string.Empty;
    public string AnswerB { get; set; } = string.Empty;
    public string AnswerC { get; set; } = string.Empty;
    public string AnswerD { get; set; } = string.Empty;
    public int CorrectAnswer { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public Guid TopicId { get; set; }
    public string TopicName { get; set; } = string.Empty;
    public Guid TeacherId { get; set; }
    public Guid LanguageId { get; set; }
    public string LanguageName { get; set; } = string.Empty;
    public Guid? ImageId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static QuestionModel MapFromEntity(Question question) => new()
    {
        Id            = question.Id,
        Title         = question.Title,
        AnswerA       = question.AnswerA,
        AnswerB       = question.AnswerB,
        AnswerC       = question.AnswerC,
        AnswerD       = question.AnswerD,
        CorrectAnswer = question.CorrectAnswer,
        Explanation   = question.Explanation,
        TopicId       = question.TopicId,
        TopicName     = question.Topic?.Name ?? string.Empty,
        TeacherId     = question.TeacherId,
        LanguageId    = question.LanguageId,
        LanguageName  = question.Language?.Name ?? string.Empty,
        ImageId       = question.ImageId,
        CreatedAt     = question.CreatedAt,
        UpdatedAt     = question.UpdatedAt,
    };
}
