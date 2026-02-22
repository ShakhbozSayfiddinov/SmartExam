using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartExam.Domain.Entities;

namespace SmartExam.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name).IsRequired().HasMaxLength(150);
        builder.Property(d => d.Description).HasMaxLength(500);

        builder.HasOne(d => d.Science)
            .WithMany(s => s.Departments)
            .HasForeignKey(d => d.ScienceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
