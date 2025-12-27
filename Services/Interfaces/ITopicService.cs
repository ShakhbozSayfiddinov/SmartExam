using SmartExam.Models.Topics;

namespace SmartExam.Services.Interfaces;

public interface ITopicService
{
    Task<TopicResponse> CreateAsync(TopicCreateRequest request, int actorUserId);
    Task<IEnumerable<TopicResponse>> GetAllAsync(int actorUserId);
    Task<TopicResponse> GetByIdAsync(int id, int actorUserId);
    Task<TopicResponse> UpdateAsync(int id, TopicUpdateRequest request, int actorUserId);
    Task DeleteAsync(int id, int actorUserId);
}
