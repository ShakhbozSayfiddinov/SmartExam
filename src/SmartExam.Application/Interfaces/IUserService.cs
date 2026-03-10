using SmartExam.Application.DTOs.Users;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IUserService
{
    Task<List<UserModel>> GetAllAsync();
    Task<UserModel> GetByIdAsync(Guid id);
    Task<UserModel> CreateAsync(CreateUserDto dto);
    Task<UserModel> UpdateAsync(Guid id, UpdateUserDto dto);
    Task DeleteAsync(Guid id);
}
