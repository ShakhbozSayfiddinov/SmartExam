using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartExam.Data;
using SmartExam.Entities;
using SmartExam.Exceptions;
using SmartExam.Models.Users;
using SmartExam.Services.Interfaces;

namespace SmartExam.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserModel> CreateAsync(UserCreateRequest request, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        EnsureAdmin(actor);

        var email = request.Email.Trim().ToLowerInvariant();
        if (await _context.Users.AnyAsync(u => u.Email == email && u.IsDeleted == false))
            throw new SmartExamException(StatusCodes.Status400BadRequest, "User with this email already exists.");

        var roleId = request.RoleId ?? await GetStudentRoleIdAsync();

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PasswordHash = HashPassword(request.Password),
            RoleId = roleId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var roleName = await _context.Roles.Where(r => r.Id == roleId).Select(r => r.Name).FirstAsync();
        return UserModel.MapFromEntity(user, roleName);
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync(int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        EnsureAdmin(actor);

        var users = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.IsDeleted == false)
            .ToListAsync();

        return users.Select(u => UserModel.MapFromEntity(u, u.Role?.Name ?? string.Empty));
    }

    public async Task<UserModel> GetByIdAsync(int userId, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false)
                   ?? throw new SmartExamException(StatusCodes.Status404NotFound, "User not found.");

        if (!IsAdmin(actor) && actor.Id != user.Id)
            throw new SmartExamException(StatusCodes.Status403Forbidden, "Not allowed to view this user.");

        return UserModel.MapFromEntity(user, user.Role?.Name ?? string.Empty);
    }

    public async Task<UserModel> UpdateAsAdminAsync(int userId, UserUpdateRequest request, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false)
                   ?? throw new SmartExamException(StatusCodes.Status404NotFound, "User not found.");

        EnsureAdmin(actor);
        await ApplyUserUpdates(user, request, allowRoleChange: true);
        var roleName = await _context.Roles.Where(r => r.Id == user.RoleId).Select(r => r.Name).FirstOrDefaultAsync() ?? string.Empty;
        return UserModel.MapFromEntity(user, roleName);
    }

    public async Task<UserModel> UpdateSelfAsync(UserUpdateRequest request, int actorUserId)
    {
        var user = await GetActorAsync(actorUserId);

        await ApplyUserUpdates(user, request, allowRoleChange: false);
        var roleName = await _context.Roles.Where(r => r.Id == user.RoleId).Select(r => r.Name).FirstOrDefaultAsync() ?? string.Empty;
        return UserModel.MapFromEntity(user, roleName);
    }

    public async Task DeleteAsAdminAsync(int userId, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false)
                   ?? throw new SmartExamException(StatusCodes.Status404NotFound, "User not found.");

        EnsureAdmin(actor);

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSelfAsync(int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        actor.IsDeleted = true;
        actor.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    private async Task<int> GetStudentRoleIdAsync()
    {
        var studentRoleId = await _context.Roles.Where(r => r.Name == "Student").Select(r => r.Id).FirstOrDefaultAsync();
        if (studentRoleId == 0)
            throw new SmartExamException(StatusCodes.Status500InternalServerError, "Default Student role not found. Seed roles first.");
        return studentRoleId;
    }

    private async Task<User> GetActorAsync(int actorUserId)
    {
        var actor = await _context.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == actorUserId && u.IsDeleted == false);

        if (actor is null)
            throw new SmartExamException(StatusCodes.Status401Unauthorized, "Actor user not found or inactive.");

        return actor;
    }

    private static bool IsAdmin(User user) => user.Role?.Name == "Admin";

    private static void EnsureAdmin(User actor)
    {
        if (!IsAdmin(actor))
            throw new SmartExamException(StatusCodes.Status403Forbidden, "Admin privileges required.");
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private async Task ApplyUserUpdates(User user, UserUpdateRequest request, bool allowRoleChange)
    {
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var exists = await _context.Users.AnyAsync(u => u.Email == email && u.Id != user.Id && u.IsDeleted == false);
            if (exists)
                throw new SmartExamException(StatusCodes.Status400BadRequest, "User with this email already exists.");
            user.Email = email;
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            user.FirstName = request.FirstName.Trim();

        if (!string.IsNullOrWhiteSpace(request.LastName))
            user.LastName = request.LastName.Trim();

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.PasswordHash = HashPassword(request.Password);

        if (allowRoleChange && request.RoleId.HasValue)
            user.RoleId = request.RoleId.Value;

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}

