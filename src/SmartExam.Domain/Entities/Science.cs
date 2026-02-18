namespace SmartExam.Domain.Entities;

public class Science : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public string Description { get; set; }

    public ICollection<Topic> Topics { get; set; } = [];
}
