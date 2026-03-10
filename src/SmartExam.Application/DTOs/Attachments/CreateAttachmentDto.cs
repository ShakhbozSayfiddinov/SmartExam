namespace SmartExam.Application.DTOs.Attachments;

public class CreateAttachmentDto
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Url { get; set; } = string.Empty;
    public Guid ZoneId { get; set; }
}
