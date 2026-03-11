using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Admin;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class AdminService(AppDbContext context) : IAdminService
{
    public async Task<PagedResult<UserModel>> GetAllUsersAsync(GetUsersQuery query)
    {
        var users = context.Users
            .Include(u => u.Role)
            .Include(u => u.Image)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Role))
            users = users.Where(u => EF.Functions.ILike(u.Role.Name, query.Role));

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            bool isDeleted = string.Equals(query.Status, "inactive", StringComparison.OrdinalIgnoreCase);
            users = users.Where(u => u.IsDeleted == isDeleted);
        }
        else
        {
            users = users.Where(u => !u.IsDeleted);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            string pattern = $"%{query.Search}%";
            users = users.Where(u =>
                EF.Functions.ILike(u.FirstName, pattern) ||
                EF.Functions.ILike(u.LastName, pattern) ||
                EF.Functions.ILike(u.PhoneNumber, pattern));
        }

        int total = await users.CountAsync();

        var items = await users
            .OrderBy(u => u.CreatedAt)
            .Skip((query.Page - 1) * query.Limit)
            .Take(query.Limit)
            .Select(u => UserModel.MapFromEntity(u))
            .ToListAsync();

        return new PagedResult<UserModel>
        {
            Items = items,
            Total = total,
            Page  = query.Page,
            Limit = query.Limit,
        };
    }
}
