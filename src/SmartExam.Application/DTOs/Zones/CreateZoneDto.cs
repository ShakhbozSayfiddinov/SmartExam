namespace SmartExam.Application.DTOs.Zones;

public class CreateZoneDto
{
    public string Name { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
}
