using SmartExam.Models.Users;

namespace SmartExam.Services.Interfaces;

public interface IUserService
{
    Task<UserResponse> CreateAsync(UserCreateRequest request, int actorUserId);
    Task<IEnumerable<UserResponse>> GetAllAsync(int actorUserId);
    Task<UserResponse> GetByIdAsync(int userId, int actorUserId);
    Task<UserResponse> UpdateAsAdminAsync(int userId, UserUpdateRequest request, int actorUserId);
    Task<UserResponse> UpdateSelfAsync(UserUpdateRequest request, int actorUserId);
    Task DeleteAsAdminAsync(int userId, int actorUserId);
    Task DeleteSelfAsync(int actorUserId);
}
