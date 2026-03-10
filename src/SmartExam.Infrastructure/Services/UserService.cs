using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Users;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class UserService(AppDbContext context) : IUserService
{
    public async Task<List<UserModel>> GetAllAsync()
    {
        var users = await context.Users
            .Include(u => u.Role)
            .Where(u => !u.IsDeleted)
            .ToListAsync();

        return users.Select(UserModel.MapFromEntity).ToList();
    }

    public async Task<UserModel> GetByIdAsync(Guid id)
    {
        var user = await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

        return user is null ? null : UserModel.MapFromEntity(user);
    }

    public async Task<UserModel> CreateAsync(CreateUserDto dto)
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

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        await context.Entry(user).Reference(u => u.Role).LoadAsync();

        return UserModel.MapFromEntity(user);
    }

    public async Task<UserModel> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted)
            ?? throw new SmartExamException(404, "error_user_not_found");

        user.FirstName   = dto.FirstName;
        user.LastName    = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        user.DateOfBirth = dto.DateOfBirth;
        user.RoleId      = dto.RoleId;
        user.UpdatedAt   = DateTime.UtcNow;

        await context.SaveChangesAsync();
        await context.Entry(user).Reference(u => u.Role).LoadAsync();

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

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
