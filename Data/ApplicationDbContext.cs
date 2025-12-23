using Microsoft.EntityFrameworkCore;
using SmartExam.Entities;

namespace SmartExam.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Science> Sciences => Set<Science>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Question> Questions => Set<Question>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureRoles(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureSciences(modelBuilder);
        ConfigureTopics(modelBuilder);
        ConfigureQuestions(modelBuilder);
    }

    private static void ConfigureRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            entity.Property(r => r.Description).HasMaxLength(500);
            entity.Property(r => r.IsActive).HasDefaultValue(true);
            entity.Property(r => r.IsDeleted).HasDefaultValue(false);
            entity.Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(r => r.Name).IsUnique();

            entity.HasMany(r => r.Users)
                .WithOne(u => u.Role!)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(r => !r.IsDeleted);
        });
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            entity.Property(u => u.IsDeleted).HasDefaultValue(false);
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(u => u.Email).IsUnique();

            entity.HasMany(u => u.Questions)
                .WithOne(q => q.CreatedBy!)
                .HasForeignKey(q => q.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasQueryFilter(u => !u.IsDeleted);
        });
    }

    private static void ConfigureSciences(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Science>(entity =>
        {
            entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            entity.Property(s => s.Description).HasMaxLength(500);
            entity.Property(s => s.IsDeleted).HasDefaultValue(false);
            entity.Property(s => s.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(s => s.Name).IsUnique();

            entity.HasMany(s => s.Topics)
                .WithOne(t => t.Science!)
                .HasForeignKey(t => t.ScienceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasQueryFilter(s => !s.IsDeleted);
        });
    }

    private static void ConfigureTopics(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Topic>(entity =>
        {
            entity.Property(t => t.Name).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).HasMaxLength(500);
            entity.Property(t => t.IsDeleted).HasDefaultValue(false);
            entity.Property(t => t.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(t => new { t.ScienceId, t.Name }).IsUnique();

            entity.HasMany(t => t.Questions)
                .WithOne(q => q.Topic!)
                .HasForeignKey(q => q.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasQueryFilter(t => !t.IsDeleted);
        });
    }

    private static void ConfigureQuestions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(q => q.Title).IsRequired().HasMaxLength(300);
            entity.Property(q => q.AnswerA).IsRequired().HasMaxLength(1000);
            entity.Property(q => q.AnswerB).IsRequired().HasMaxLength(1000);
            entity.Property(q => q.AnswerC).IsRequired().HasMaxLength(1000);
            entity.Property(q => q.AnswerD).IsRequired().HasMaxLength(1000);
            entity.Property(q => q.CorrectAnswer).IsRequired().HasMaxLength(1);
            entity.Property(q => q.Explation).HasMaxLength(2000);
            entity.Property(q => q.IsDeleted).HasDefaultValue(false);
            entity.Property(q => q.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasQueryFilter(q => !q.IsDeleted);
        });
    }
}
