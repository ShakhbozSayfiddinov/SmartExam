using SmartExam.Domain.Entities;

namespace SmartExam.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
