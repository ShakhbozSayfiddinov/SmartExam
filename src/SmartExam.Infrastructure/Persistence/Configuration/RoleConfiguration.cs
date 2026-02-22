using Microsoft.EntityFrameworkCore;
using SmartExam.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartExam.Infrastructure.Persistence.Configuration;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(r => r.Description)
            .HasMaxLength(200);
        builder.Property(r => r.IsDeleted)
            .IsRequired();
    }
}