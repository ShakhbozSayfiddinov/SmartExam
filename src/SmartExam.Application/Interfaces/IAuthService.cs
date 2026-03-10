using SmartExam.Application.DTOs.Auth;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IAuthService
{
    Task<AuthModel> RegisterAsync(RegisterDto dto);
    Task<AuthModel> LoginAsync(LoginDto dto);
}
