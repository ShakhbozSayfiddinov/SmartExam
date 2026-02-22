using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartExam.Domain.Entities;

namespace SmartExam.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FileName).IsRequired().HasMaxLength(255);
        builder.Property(a => a.ContentType).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Url).IsRequired().HasMaxLength(1000);

        builder.HasOne(a => a.Zone)
            .WithMany(z => z.Attachments)
            .HasForeignKey(a => a.ZoneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
