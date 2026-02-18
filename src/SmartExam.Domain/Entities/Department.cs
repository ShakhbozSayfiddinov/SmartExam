namespace SmartExam.Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public Guid ScienceId { get; set; }
    public Science Science { get; set; }

    public ICollection<Topic> Topics { get; set; } = [];
}
