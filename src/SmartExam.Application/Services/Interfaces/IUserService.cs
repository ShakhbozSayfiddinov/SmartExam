using SmartExam.Models.Users;

namespace SmartExam.Services.Interfaces;

public interface IUserService
{
    Task<UserModel> CreateAsync(UserCreateRequest request, int actorUserId);
    Task<IEnumerable<UserModel>> GetAllAsync(int actorUserId);
    Task<UserModel> GetByIdAsync(int userId, int actorUserId);
    Task<UserModel> UpdateAsAdminAsync(int userId, UserUpdateRequest request, int actorUserId);
    Task<UserModel> UpdateSelfAsync(UserUpdateRequest request, int actorUserId);
    Task DeleteAsAdminAsync(int userId, int actorUserId);
    Task DeleteSelfAsync(int actorUserId);
}

