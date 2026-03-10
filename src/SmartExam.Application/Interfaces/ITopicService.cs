using SmartExam.Application.DTOs.Topics;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface ITopicService
{
    Task<List<TopicModel>> GetAllAsync();
    Task<TopicModel> GetByIdAsync(Guid id);
    Task<TopicModel> CreateAsync(CreateTopicDto dto);
    Task<TopicModel> UpdateAsync(Guid id, UpdateTopicDto dto);
    Task DeleteAsync(Guid id);
}
