using SmartExam.Application.DTOs.Students;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IStudentService
{
    Task<List<StudentModel>> GetAllAsync();
    Task<StudentModel> GetByIdAsync(Guid id);
    Task<StudentModel> CreateAsync(CreateStudentDto dto);
    Task DeleteAsync(Guid id);
}
