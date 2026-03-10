using SmartExam.Application.DTOs.Sciences;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IScienceService
{
    Task<List<ScienceModel>> GetAllAsync();
    Task<ScienceModel> GetByIdAsync(Guid id);
    Task<ScienceModel> CreateAsync(CreateScienceDto dto);
    Task<ScienceModel> UpdateAsync(Guid id, UpdateScienceDto dto);
    Task DeleteAsync(Guid id);
}
