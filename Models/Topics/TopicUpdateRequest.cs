namespace SmartExam.Models.Topics;

public class TopicUpdateRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? ScienceId { get; set; }
}
