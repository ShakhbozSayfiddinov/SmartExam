namespace SmartExam.Domain.Entities;

public class Zone : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Attachment> Attachments { get; set; } = [];
}
