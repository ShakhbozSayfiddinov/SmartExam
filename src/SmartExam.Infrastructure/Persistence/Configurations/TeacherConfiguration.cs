using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartExam.Domain.Entities;

namespace SmartExam.Infrastructure.Persistence.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasMany(t => t.Questions)
            .WithOne(q => q.Teacher)
            .HasForeignKey(q => q.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
