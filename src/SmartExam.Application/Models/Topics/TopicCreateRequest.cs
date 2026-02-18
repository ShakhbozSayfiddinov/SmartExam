#nullable enable
namespace SmartExam.Application.Models.Topics;

public class TopicCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ScienceId { get; set; }
}
