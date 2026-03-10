namespace SmartExam.Application.DTOs.Topics;

public class UpdateTopicDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}
