using SmartExam.Models.Questions;

namespace SmartExam.Services.Interfaces;

public interface IQuestionService
{
    Task<QuestionResponse> CreateAsync(QuestionCreateRequest request, int actorUserId);
    Task<IEnumerable<QuestionResponse>> GetAllAsync(int actorUserId);
    Task<QuestionResponse> GetByIdAsync(int id, int actorUserId);
    Task<QuestionResponse> UpdateAsync(int id, QuestionUpdateRequest request, int actorUserId);
    Task DeleteAsync(int id, int actorUserId);
}
