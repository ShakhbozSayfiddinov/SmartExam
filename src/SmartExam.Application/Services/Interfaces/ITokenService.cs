using SmartExam.Entities;

namespace SmartExam.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
