using SmartExam.Application.DTOs.Languages;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface ILanguageService
{
    Task<List<LanguageModel>> GetAllAsync();
    Task<LanguageModel> GetByIdAsync(Guid id);
    Task<LanguageModel> CreateAsync(CreateLanguageDto dto);
    Task<LanguageModel> UpdateAsync(Guid id, UpdateLanguageDto dto);
    Task DeleteAsync(Guid id);
}
