using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SmartExam.Domain.Entities;

namespace SmartExam.Infrastructure.Persistence.Seeders;

public static class DataSeeder
{
    // Fixed GUIDs — migration da o'zgarmasin
    public static readonly Guid AvatarsZoneId = Guid.Parse("cccccccc-0000-0000-0000-000000000000");

    private static readonly Guid SystemAdminRoleId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000000");
    private static readonly Guid AdminRoleId       = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");
    private static readonly Guid TeacherRoleId     = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002");
    private static readonly Guid StudentRoleId     = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000003");

    private static readonly Guid SystemAdminUserId = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000000");
    private static readonly Guid AdminUserId       = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001");

    private static readonly string[] SystemAdminPermissions =
    [
        "Users.Read",       "Users.Write",
        "Roles.Read",       "Roles.Write",
        "Permissions.Read", "Permissions.Write",
        "Questions.Read",   "Questions.Write",
        "Exams.Read",       "Exams.Write",
        "Sciences.Read",    "Sciences.Write",
        "Departments.Read", "Departments.Write",
        "Topics.Read",      "Topics.Write",
        "Languages.Read",   "Languages.Write",
        "Reports.Read",     "Reports.Write",
    ];

    private static readonly string[] AdminPermissions =
    [
        "Users.Read",       "Users.Write",
        "Questions.Read",   "Questions.Write",
        "Exams.Read",       "Exams.Write",
        "Sciences.Read",    "Sciences.Write",
        "Departments.Read", "Departments.Write",
        "Topics.Read",      "Topics.Write",
        "Languages.Read",
        "Reports.Read",
    ];

    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedAvatarsZoneAsync(context);
        await SeedLanguagesAsync(context);
        await SeedRolesAsync(context);
        await SeedPermissionsAsync(context);
        await SeedSystemAdminUserAsync(context);
        await SeedAdminUserAsync(context);
    }

    private static async Task SeedAvatarsZoneAsync(AppDbContext context)
    {
        if (await context.Zones.AnyAsync(z => z.Id == AvatarsZoneId)) return;

        await context.Zones.AddAsync(new Zone
        {
            Id     = AvatarsZoneId,
            Name   = "Avatars",
            Width  = 0,
            Height = 0,
        });

        await context.SaveChangesAsync();
    }

    private static async Task SeedLanguagesAsync(AppDbContext context)
    {
        if (await context.Languages.AnyAsync()) return;

        var languages = new List<Language>
        {
            new() { Id = Guid.NewGuid(), Name = "O'zbek",  Code = "uz" },
            new() { Id = Guid.NewGuid(), Name = "Русский", Code = "ru" },
            new() { Id = Guid.NewGuid(), Name = "English", Code = "en" },
        };

        await context.Languages.AddRangeAsync(languages);
        await context.SaveChangesAsync();
    }

    private static async Task SeedRolesAsync(AppDbContext context)
    {
        if (await context.Roles.AnyAsync()) return;

        var roles = new List<Role>
        {
            new() { Id = SystemAdminRoleId, Name = "SystemAdmin", Description = "Tizim super admini", IsActive = true },
            new() { Id = AdminRoleId,       Name = "Admin",       Description = "Tizim admini",       IsActive = true },
            new() { Id = TeacherRoleId,     Name = "Teacher",     Description = "O'qituvchi",         IsActive = true },
            new() { Id = StudentRoleId,     Name = "Student",     Description = "O'quvchi",           IsActive = true },
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPermissionsAsync(AppDbContext context)
    {
        if (await context.Permissions.AnyAsync()) return;

        var allNames = SystemAdminPermissions.Union(AdminPermissions).Distinct();

        var permissions = allNames.Select(name => new Permission
        {
            Id          = Guid.NewGuid(),
            Name        = name,
            Description = name,
        }).ToList();

        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();

        var saved = await context.Permissions.ToListAsync();

        var rolePermissions = new List<RolePermission>();

        rolePermissions.AddRange(
            saved.Select(p => new RolePermission { RoleId = SystemAdminRoleId, PermissionId = p.Id }));

        rolePermissions.AddRange(
            saved.Where(p => AdminPermissions.Contains(p.Name))
                 .Select(p => new RolePermission { RoleId = AdminRoleId, PermissionId = p.Id }));

        await context.RolePermissions.AddRangeAsync(rolePermissions);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSystemAdminUserAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.PhoneNumber == "+998000000001")) return;

        var systemAdmin = new User
        {
            Id           = SystemAdminUserId,
            FirstName    = "System",
            LastName     = "Admin",
            PhoneNumber  = "+998000000001",
            PasswordHash = HashPassword("SysAdmin@123"),
            RoleId       = SystemAdminRoleId,
            CreatedAt    = DateTime.UtcNow,
            IsDeleted    = false,
        };

        await context.Users.AddAsync(systemAdmin);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAdminUserAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.PhoneNumber == "+998000000002")) return;

        var admin = new User
        {
            Id           = AdminUserId,
            FirstName    = "Admin",
            LastName     = "Admin",
            PhoneNumber  = "+998000000002",
            PasswordHash = HashPassword("Qwerty123$"),
            RoleId       = AdminRoleId,
            IsDeleted    = false,
        };

        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
