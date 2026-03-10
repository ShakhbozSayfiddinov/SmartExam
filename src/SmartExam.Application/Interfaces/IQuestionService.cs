using SmartExam.Application.DTOs.Questions;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IQuestionService
{
    Task<List<QuestionModel>> GetAllAsync();
    Task<QuestionModel> GetByIdAsync(Guid id);
    Task<QuestionModel> CreateAsync(CreateQuestionDto dto);
    Task<QuestionModel> UpdateAsync(Guid id, UpdateQuestionDto dto);
    Task DeleteAsync(Guid id);
}
