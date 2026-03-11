using SmartExam.Application.DTOs.Users;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IUserService
{
    Task<List<UserModel>> GetAllAsync();
    Task<UserModel> GetByIdAsync(Guid id);
    Task<UserModel> CreateAsync(CreateUserDto dto, string imageUrl = null);
    Task<UserModel> UpdateAsync(Guid id, UpdateUserDto dto, string imageUrl = null);
    Task DeleteAsync(Guid id);
}
