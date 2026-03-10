using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Topics;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class TopicService(AppDbContext context) : ITopicService
{
    public async Task<List<TopicModel>> GetAllAsync()
    {
        var topics = await context.Topics
            .Include(t => t.Department)
            .Where(t => !t.IsDeleted)
            .ToListAsync();

        return topics.Select(TopicModel.MapFromEntity)?.ToList();
    }

    public async Task<TopicModel> GetByIdAsync(Guid id)
    {
        var topic = await context.Topics
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        return topic is null ? null : TopicModel.MapFromEntity(topic);
    }

    public async Task<TopicModel> CreateAsync(CreateTopicDto dto)
    {
        var departmentExists = await context.Departments.AnyAsync(d => d.Id == dto.DepartmentId && !d.IsDeleted);
        if (!departmentExists)
            throw new SmartExamException(404, "error_department_not_found");

        var topic = new Topic
        {
            Name         = dto.Name,
            Description  = dto.Description,
            DepartmentId = dto.DepartmentId,
        };

        await context.Topics.AddAsync(topic);
        await context.SaveChangesAsync();
        await context.Entry(topic).Reference(t => t.Department).LoadAsync();

        return TopicModel.MapFromEntity(topic);
    }

    public async Task<TopicModel> UpdateAsync(Guid id, UpdateTopicDto dto)
    {
        var topic = await context.Topics
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
            ?? throw new SmartExamException(404, "error_topic_not_found");

        var departmentExists = await context.Departments.AnyAsync(d => d.Id == dto.DepartmentId && !d.IsDeleted);
        if (!departmentExists)
            throw new SmartExamException(404, "error_department_not_found");

        topic.Name         = dto.Name;
        topic.Description  = dto.Description;
        topic.DepartmentId = dto.DepartmentId;
        topic.UpdatedAt    = DateTime.UtcNow;

        await context.SaveChangesAsync();
        await context.Entry(topic).Reference(t => t.Department).LoadAsync();

        return TopicModel.MapFromEntity(topic);
    }

    public async Task DeleteAsync(Guid id)
    {
        var topic = await context.Topics
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
            ?? throw new SmartExamException(404, "error_topic_not_found");

        topic.IsDeleted = true;
        topic.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
