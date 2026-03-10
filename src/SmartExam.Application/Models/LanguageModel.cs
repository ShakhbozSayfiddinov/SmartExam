using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class LanguageModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static LanguageModel MapFromEntity(Language language) => new()
    {
        Id        = language.Id,
        Name      = language.Name,
        Code      = language.Code,
        CreatedAt = language.CreatedAt,
        UpdatedAt = language.UpdatedAt,
    };
}
