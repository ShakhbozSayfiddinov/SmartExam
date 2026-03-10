using SmartExam.Application.DTOs.Departments;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentModel>> GetAllAsync();
    Task<DepartmentModel> GetByIdAsync(Guid id);
    Task<DepartmentModel> CreateAsync(CreateDepartmentDto dto);
    Task<DepartmentModel> UpdateAsync(Guid id, UpdateDepartmentDto dto);
    Task DeleteAsync(Guid id);
}
