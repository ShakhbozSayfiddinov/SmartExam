namespace SmartExam.Application.DTOs.Sciences;

public class CreateScienceDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ScienceId { get; set; }
}
