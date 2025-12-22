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

            var defaultStudent = new User
            {
                FirstName = "Default",
                LastName = "Student",
                Email = "student@smartexam.test",
                PasswordHash = "changeme", // TODO: replace with hashed password
                RoleId = studentRoleId
            };

            await context.Users.AddAsync(defaultStudent);
            await context.SaveChangesAsync();
        }
    }
}
