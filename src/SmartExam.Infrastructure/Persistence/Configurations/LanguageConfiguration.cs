using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartExam.Domain.Entities;

namespace SmartExam.Infrastructure.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name).IsRequired().HasMaxLength(50);
        builder.Property(l => l.Code).IsRequired().HasMaxLength(5);

        builder.HasIndex(l => l.Code).IsUnique();
    }
}
