using SmartExam.Application.DTOs.Teachers;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface ITeacherService
{
    Task<List<TeacherModel>> GetAllAsync();
    Task<TeacherModel> GetByIdAsync(Guid id);
    Task<TeacherModel> CreateAsync(CreateTeacherDto dto);
    Task DeleteAsync(Guid id);
}
