namespace SmartExam.Models.Topics;

public class TopicResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ScienceId { get; set; }
    public DateTime CreatedAt { get; set; }
}
