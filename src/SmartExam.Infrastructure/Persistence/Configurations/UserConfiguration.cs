using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartExam.Domain.Entities;

namespace SmartExam.Infrastructure.Persistence.Configurations;
public class UserConfiguration :IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);
        builder.Property(u => u.PasswordHash)
            .IsRequired();
        
        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(u => u.Image)
            .WithMany()
            .HasForeignKey(u => u.ImageId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(u => u.Teacher)
            .WithOne(t => t.User)
            .HasForeignKey<Teacher>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Student)
            .WithOne(s => s.User)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}