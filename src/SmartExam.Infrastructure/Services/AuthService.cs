using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartExam.Application.DTOs.Auth;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
    public async Task<AuthModel> RegisterAsync(RegisterDto dto)
    {
        var studentRole = await context.Roles
            .FirstOrDefaultAsync(r => r.Name == "Student")
            ?? throw new SmartExamException(404, "error_student_role_not_found");

        var user = new User
        {
            Id           = Guid.NewGuid(),
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            PhoneNumber  = dto.PhoneNumber,
            PasswordHash = HashPassword(dto.Password),
            DateOfBirth  = dto.DateOfBirth,
            RoleId       = studentRole.Id,
            CreatedAt    = DateTime.UtcNow,
            IsDeleted    = false,
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        await context.Students.AddAsync(new Student { Id = Guid.NewGuid(), UserId = user.Id });
        await context.SaveChangesAsync();

        return GenerateToken(user, studentRole.Name);
    }

    public async Task<AuthModel> LoginAsync(LoginDto dto)
    {
        var user = await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber && !u.IsDeleted)
            ?? throw new SmartExamException(401, "error_invalid_credentials");

        if (user.PasswordHash != HashPassword(dto.Password))
            throw new SmartExamException(401, "error_invalid_credentials");

        return GenerateToken(user, user.Role.Name);
    }

    private AuthModel GenerateToken(User user, string roleName)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var key        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var expires    = DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["AccessTokenExpiresMinutes"]!));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, roleName),
        };

        var token = new JwtSecurityToken(
            issuer:             jwtSection["Issuer"],
            audience:           jwtSection["Audience"],
            claims:             claims,
            expires:            expires,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new AuthModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt   = expires,
            UserId      = user.Id,
            FullName    = $"{user.FirstName} {user.LastName}",
            RoleName    = roleName,
        };
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
