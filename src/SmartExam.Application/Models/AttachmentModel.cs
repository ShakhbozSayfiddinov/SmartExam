using SmartExam.Domain.Entities;

namespace SmartExam.Application.Models;

public class AttachmentModel
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Url { get; set; } = string.Empty;
    public Guid ZoneId { get; set; }
    public string ZoneName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static AttachmentModel MapFromEntity(Attachment attachment) => new()
    {
        Id          = attachment.Id,
        FileName    = attachment.FileName,
        ContentType = attachment.ContentType,
        FileSize    = attachment.FileSize,
        Url         = attachment.Url,
        ZoneId      = attachment.ZoneId,
        ZoneName    = attachment.Zone?.Name ?? string.Empty,
        CreatedAt   = attachment.CreatedAt,
        UpdatedAt   = attachment.UpdatedAt,
    };
}
