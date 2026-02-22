using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartExam.Domain.Entities;

namespace SmartExam.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Title).IsRequired().HasMaxLength(500);
        builder.Property(q => q.AnswerA).IsRequired().HasMaxLength(300);
        builder.Property(q => q.AnswerB).IsRequired().HasMaxLength(300);
        builder.Property(q => q.AnswerC).IsRequired().HasMaxLength(300);
        builder.Property(q => q.AnswerD).IsRequired().HasMaxLength(300);
        builder.Property(q => q.Explation).HasMaxLength(1000);

        builder.HasOne(q => q.Topic)
            .WithMany(t => t.Questions)
            .HasForeignKey(q => q.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(q => q.Image)
            .WithMany()
            .HasForeignKey(q => q.ImageId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
