using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartExam.Domain.Entities;

namespace SmartExam.Infrastructure.Persistence.Configurations;

public class ScienceConfiguration : IEntityTypeConfiguration<Science>
{
    public void Configure(EntityTypeBuilder<Science> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Description).HasMaxLength(500);

        builder.HasOne(s => s.ParentScience)
            .WithMany(s => s.ChildSciences)
            .HasForeignKey(s => s.ScienceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
