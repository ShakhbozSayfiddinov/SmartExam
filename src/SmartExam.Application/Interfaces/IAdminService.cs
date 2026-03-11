using SmartExam.Application.DTOs.Admin;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IAdminService
{
    Task<PagedResult<UserModel>> GetAllUsersAsync(GetUsersQuery query);
}
