using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Users;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;
using SmartExam.Infrastructure.Persistence.Seeders;

namespace SmartExam.Infrastructure.Services;

public class UserService(AppDbContext context) : IUserService
{
    public async Task<List<UserModel>> GetAllAsync()
    {
        var users = await context.Users
            .Include(u => u.Role)
            .Include(u => u.Image)
            .Where(u => !u.IsDeleted)
            .ToListAsync();

        return users.Select(UserModel.MapFromEntity)?.ToList();
    }

    public async Task<UserModel> GetByIdAsync(Guid id)
    {
        var user = await context.Users
            .Include(u => u.Role)
            .Include(u => u.Image)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

        return user is null ? null : UserModel.MapFromEntity(user);
    }

    public async Task<UserModel> CreateAsync(CreateUserDto dto, string imageUrl = null)
    {
        var user = new User
        {
            Id           = Guid.NewGuid(),
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            PhoneNumber  = dto.PhoneNumber,
            PasswordHash = HashPassword(dto.Password),
            DateOfBirth  = dto.DateOfBirth,
            RoleId       = dto.RoleId,
            CreatedAt    = DateTime.UtcNow,
            IsDeleted    = false,
        };

        if (imageUrl is not null)
        {
            var attachment = new Attachment
            {
                Id          = Guid.NewGuid(),
                FileName    = Path.GetFileName(imageUrl),
                ContentType = "image/*",
                FileSize    = 0,
                Url         = imageUrl,
                ZoneId      = DataSeeder.AvatarsZoneId,
                CreatedAt   = DateTime.UtcNow,
            };
            await context.Attachments.AddAsync(attachment);
            await context.SaveChangesAsync();
            user.ImageId = attachment.Id;
        }

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        await CreateProfileAsync(dto.RoleId, user.Id);
        await context.SaveChangesAsync();
        await context.Entry(user).Reference(u => u.Role).LoadAsync();
        await context.Entry(user).Reference(u => u.Image).LoadAsync();

        return UserModel.MapFromEntity(user);
    }

    public async Task<UserModel> UpdateAsync(Guid id, UpdateUserDto dto, string imageUrl = null)
    {
        var user = await context.Users
            .Include(u => u.Role)
            .Include(u => u.Image)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted)
            ?? throw new SmartExamException(404, "error_user_not_found");

        Guid oldRoleId = user.RoleId;

        user.FirstName   = dto.FirstName;
        user.LastName    = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        user.DateOfBirth = dto.DateOfBirth;
        user.RoleId      = dto.RoleId;
        user.UpdatedAt   = DateTime.UtcNow;

        if (oldRoleId != dto.RoleId)
        {
            await UpdateTeacherStatusAsync(oldRoleId, id, isDeleted: true);
            await UpdateStudentStatusAsync(oldRoleId, id, isDeleted: true);
            await CreateProfileAsync(dto.RoleId, id);
        }

        if (imageUrl is not null)
        {
            if (user.Image is not null)
            {
                user.Image.Url      = imageUrl;
                user.Image.FileName = Path.GetFileName(imageUrl);
            }
            else
            {
                var attachment = new Attachment
                {
                    Id          = Guid.NewGuid(),
                    FileName    = Path.GetFileName(imageUrl),
                    ContentType = "image/*",
                    FileSize    = 0,
                    Url         = imageUrl,
                    ZoneId      = DataSeeder.AvatarsZoneId,
                    CreatedAt   = DateTime.UtcNow,
                };
                await context.Attachments.AddAsync(attachment);
                await context.SaveChangesAsync();
                user.ImageId = attachment.Id;
            }
        }

        await context.SaveChangesAsync();

        return UserModel.MapFromEntity(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted)
            ?? throw new SmartExamException(404, "error_user_not_found");

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    private async Task CreateTeacherAsync(Guid userId)
        => await context.Teachers.AddAsync(new Teacher { Id = Guid.NewGuid(), UserId = userId });

    private async Task CreateStudentAsync(Guid userId)
        => await context.Students.AddAsync(new Student { Id = Guid.NewGuid(), UserId = userId });

    private async Task CreateProfileAsync(Guid roleId, Guid userId)
    {
        if (roleId == DataSeeder.TeacherRoleId)
            await CreateTeacherAsync(userId);
        else if (roleId == DataSeeder.StudentRoleId)
            await CreateStudentAsync(userId);
    }

    private async Task UpdateTeacherStatusAsync(Guid roleId, Guid userId, bool isDeleted)
    {
        if (roleId != DataSeeder.TeacherRoleId) return;

        var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId && t.IsDeleted != isDeleted);
        if (teacher is not null) teacher.IsDeleted = isDeleted;
    }

    private async Task UpdateStudentStatusAsync(Guid roleId, Guid userId, bool isDeleted)
    {
        if (roleId != DataSeeder.StudentRoleId) return;

        var student = await context.Students.FirstOrDefaultAsync(s => s.UserId == userId && s.IsDeleted != isDeleted);
        if (student is not null) student.IsDeleted = isDeleted;
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
