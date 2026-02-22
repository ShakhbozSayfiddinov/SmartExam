namespace SmartExam.Domain.Entities;

public class Attachment : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public Guid ZoneId { get; set; }
    public Zone Zone { get; set; }
}
