using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartExam.Data;
using SmartExam.Domain.Entities;
using SmartExam.Exceptions;
using SmartExam.Application.Models.Auth;
using SmartExam.Application.Services.Interfaces;
using SmartExam.Infrastructure.Data;

namespace SmartExam.Infrastructure.Services;

public class AuthService(ApplicationDbContext context, ITokenService tokenService) : IAuthService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var phoneNumber = request.PhoneNumber.Trim().ToLowerInvariant();

        if (await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber && !u.IsDeleted))
        {
            throw new SmartExamException(StatusCodes.Status400BadRequest, "User with this phone number already exists.");
        }

        var studentRoleId = await _context.Roles
            .Where(r => r.Name == "Student") 
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        if (studentRoleId == 0)
            throw new SmartExamException(StatusCodes.Status500InternalServerError, "Default Student role not found. Seed roles first.");

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            PhoneNumber = phoneNumber,
            PasswordHash = HashPassword(request.Password),
            RoleId = studentRoleId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var roleName = await _context.Roles.Where(r => r.Id == studentRoleId).Select(r => r.Name).FirstAsync();
        user.Role = new Role { Id = studentRoleId, Name = roleName };

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var phoneNumber = request.PhoneNumber.Trim().ToLowerInvariant();
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.IsDeleted == false);

        if (user is null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new SmartExamException(StatusCodes.Status401Unauthorized, "Invalid phone number or password.");
        }

        return BuildAuthResponse(user);
    }

    private static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    private static bool VerifyPassword(string password, string storedHash) =>
        HashPassword(password) == storedHash;

    private AuthResponse BuildAuthResponse(User user)
    {
        var token = _tokenService.GenerateToken(user);
        return new AuthResponse
        {
            UserId = user.Id,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            PhoneNumber = user.PhoneNumber,
            Role = user.Role?.Name ?? string.Empty,
            Token = token
        };
    }
}
