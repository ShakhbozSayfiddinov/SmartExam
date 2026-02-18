using Microsoft.EntityFrameworkCore;
using SmartExam.Domain.Entities;

namespace SmartExam.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Science> Sciences => Set<Science>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureRoles(modelBuilder);
        ConfigurePermissions(modelBuilder);
        ConfigureRolePermissions(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureAttachments(modelBuilder);
        ConfigureSciences(modelBuilder);
        ConfigureDepartments(modelBuilder);
        ConfigureTopics(modelBuilder);
        ConfigureQuestions(modelBuilder);
    }

    private static void ConfigureRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(r => r.Name).IsRequired().HasMaxLength(255);
            entity.Property(r => r.Description).HasMaxLength(255);
            entity.Property(r => r.IsActive).HasDefaultValue(true);
            entity.Property(r => r.IsDeleted).HasDefaultValue(false);
            entity.Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(r => !r.IsDeleted);
        });
    }

    private static void ConfigurePermissions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.Property(p => p.Name).IsRequired().HasMaxLength(255);
            entity.Property(p => p.Description).HasMaxLength(255);
            entity.Property(p => p.IsDeleted).HasDefaultValue(false);
            entity.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(p => p.Name).IsUnique();

            entity.HasQueryFilter(p => !p.IsDeleted);
        });
    }

    private static void ConfigureRolePermissions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            entity.Property(rp => rp.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(255);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(255);
            entity.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(255);
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(u => u.IsDeleted).HasDefaultValue(false);
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(u => u.PhoneNumber).IsUnique();

            entity.HasOne(u => u.Image)
                .WithMany()
                .HasForeignKey(u => u.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(u => u.Questions)
                .WithOne(q => q.CreatedBy)
                .HasForeignKey(q => q.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasQueryFilter(u => !u.IsDeleted);
        });
    }

    private static void ConfigureAttachments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.Property(a => a.FileName).IsRequired().HasMaxLength(255);
            entity.Property(a => a.ContentType).IsRequired().HasMaxLength(150);
            entity.Property(a => a.Url).IsRequired();
            entity.Property(a => a.IsDeleted).HasDefaultValue(false);
            entity.Property(a => a.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasQueryFilter(a => !a.IsDeleted);
        });
    }

    private static void ConfigureSciences(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Science>(entity =>
        {
            entity.Property(s => s.Name).IsRequired().HasMaxLength(255);
            entity.Property(s => s.Description).IsRequired();
            entity.Property(s => s.IsDeleted).HasDefaultValue(false);
            entity.Property(s => s.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(s => s.ParentScience)
                .WithMany(s => s.ChildSciences)
                .HasForeignKey(s => s.ScienceId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            entity.HasMany(s => s.Departments)
                .WithOne(d => d.Science)
                .HasForeignKey(d => d.ScienceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasQueryFilter(s => !s.IsDeleted);
        });
    }

    private static void ConfigureDepartments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name).IsRequired().HasMaxLength(255);
            entity.Property(d => d.Description).HasMaxLength(255);
            entity.Property(d => d.IsDeleted).HasDefaultValue(false);
            entity.Property(d => d.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasMany(d => d.Topics)
                .WithOne(t => t.Department)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasQueryFilter(d => !d.IsDeleted);
        });
    }

    private static void ConfigureTopics(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Topic>(entity =>
        {
            entity.Property(t => t.Name).IsRequired().HasMaxLength(255);
            entity.Property(t => t.Description).HasMaxLength(255);
            entity.Property(t => t.IsDeleted).HasDefaultValue(false);
            entity.Property(t => t.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasMany(t => t.Questions)
                .WithOne(q => q.Topic)
                .HasForeignKey(q => q.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasQueryFilter(t => !t.IsDeleted);
        });
    }

    private static void ConfigureQuestions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(q => q.Title).IsRequired();
            entity.Property(q => q.AnswerA).IsRequired().HasMaxLength(255);
            entity.Property(q => q.AnswerB).IsRequired().HasMaxLength(255);
            entity.Property(q => q.AnswerC).IsRequired().HasMaxLength(255);
            entity.Property(q => q.AnswerD).IsRequired().HasMaxLength(255);
            entity.Property(q => q.CorrectAnswer).IsRequired();
            entity.Property(q => q.Explation).IsRequired();
            entity.Property(q => q.IsDeleted).HasDefaultValue(false);
            entity.Property(q => q.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(q => q.Image)
                .WithMany()
                .HasForeignKey(q => q.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasQueryFilter(q => !q.IsDeleted);
        });
    }
}
