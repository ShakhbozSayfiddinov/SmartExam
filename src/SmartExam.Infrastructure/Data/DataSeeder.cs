using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartExam.Entities;

namespace SmartExam.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureCreatedAsync();

        if (!await context.Roles.AnyAsync())
        {
            var adminRole = new Role { Name = "Admin", Description = "System administrator" };
            var teacherRole = new Role { Name = "Teacher", Description = "Teacher role" };
            var studentRole = new Role { Name = "Student", Description = "Default role for new users" };

            await context.Roles.AddRangeAsync(adminRole, teacherRole, studentRole);
            await context.SaveChangesAsync();
        }

        if (!await context.Users.AnyAsync())
        {
            var studentRoleId = await context.Roles.Where(r => r.Name == "Student").Select(r => r.Id).FirstAsync();
            var adminRoleId = await context.Roles.Where(r => r.Name == "Admin").Select(r => r.Id).FirstAsync();

            var defaultStudent = new User
            {
                FirstName = "Admin",
                LastName = "Admin",
                PhoneNumber = "+998997476017",
                PasswordHash = HashPassword("Qwerty123$"),
                RoleId = adminRoleId,
            };

            await context.Users.AddAsync(defaultStudent);
            await context.SaveChangesAsync();
        }

        var generalScience = await context.Sciences.FirstOrDefaultAsync(s => s.Name == "General");
        if (generalScience is null)
        {
            generalScience = new Science { Name = "General", Description = "General science", IsDeleted = false };
            await context.Sciences.AddAsync(generalScience);
            await context.SaveChangesAsync();
        }

        var hasGeneralTopic = await context.Topics.AnyAsync(t => t.Name == "General" && t.ScienceId == generalScience.Id);
        if (!hasGeneralTopic)
        {
            var generalTopic = new Topic
            {
                Name = "General",
                Description = "General topic",
                ScienceId = generalScience.Id
            };
            await context.Topics.AddAsync(generalTopic);
            await context.SaveChangesAsync();
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
