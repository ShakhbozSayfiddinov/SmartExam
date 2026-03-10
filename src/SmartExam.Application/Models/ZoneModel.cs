using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class ZoneModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static ZoneModel MapFromEntity(Zone zone) => new()
    {
        Id        = zone.Id,
        Name      = zone.Name,
        Width     = zone.Width,
        Height    = zone.Height,
        CreatedAt = zone.CreatedAt,
        UpdatedAt = zone.UpdatedAt,
    };
}
