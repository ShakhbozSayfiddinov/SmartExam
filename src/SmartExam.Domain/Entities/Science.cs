namespace SmartExam.Domain.Entities;

public class Science : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public Guid? ScienceId { get; set; }
    public Science? ParentScience { get; set; }

    public ICollection<Science> ChildSciences { get; set; } = [];
    public ICollection<Department> Departments { get; set; } = [];
}
