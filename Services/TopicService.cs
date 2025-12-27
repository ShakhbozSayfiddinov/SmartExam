using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartExam.Data;
using SmartExam.Entities;
using SmartExam.Exceptions;
using SmartExam.Models.Topics;
using SmartExam.Services.Interfaces;

namespace SmartExam.Services;

public class TopicService : ITopicService
{
    private readonly ApplicationDbContext _context;

    public TopicService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TopicResponse> CreateAsync(TopicCreateRequest request, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        EnsureTeacherOrAdmin(actor);

        await EnsureScienceExists(request.ScienceId);
        await EnsureTopicNameUnique(request.Name, request.ScienceId);

        var topic = new Topic
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            ScienceId = request.ScienceId
        };

        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();

        return MapToResponse(topic);
    }

    public async Task<IEnumerable<TopicResponse>> GetAllAsync(int actorUserId)
    {
        await GetActorAsync(actorUserId);

        var topics = await _context.Topics.AsNoTracking().ToListAsync();
        return topics.Select(MapToResponse);
    }

    public async Task<TopicResponse> GetByIdAsync(int id, int actorUserId)
    {
        await GetActorAsync(actorUserId);

        var topic = await _context.Topics.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new SmartExamException(StatusCodes.Status404NotFound, "Topic not found.");

        return MapToResponse(topic);
    }

    public async Task<TopicResponse> UpdateAsync(int id, TopicUpdateRequest request, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        EnsureTeacherOrAdmin(actor);

        var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new SmartExamException(StatusCodes.Status404NotFound, "Topic not found.");

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var newName = request.Name.Trim();
            var scienceIdForName = request.ScienceId ?? topic.ScienceId;
            await EnsureTopicNameUnique(newName, scienceIdForName, topic.Id);
            topic.Name = newName;
        }

        if (request.Description is not null)
            topic.Description = request.Description.Trim();

        if (request.ScienceId.HasValue && request.ScienceId.Value != topic.ScienceId)
        {
            await EnsureScienceExists(request.ScienceId.Value);
            await EnsureTopicNameUnique(topic.Name, request.ScienceId.Value, topic.Id);
            topic.ScienceId = request.ScienceId.Value;
        }

        topic.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapToResponse(topic);
    }

    public async Task DeleteAsync(int id, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        EnsureTeacherOrAdmin(actor);

        var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new SmartExamException(StatusCodes.Status404NotFound, "Topic not found.");

        topic.IsDeleted = true;
        topic.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    private async Task<User> GetActorAsync(int actorUserId)
    {
        var actor = await _context.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == actorUserId && u.IsDeleted == false);

        if (actor is null)
            throw new SmartExamException(StatusCodes.Status401Unauthorized, "Actor user not found or inactive.");

        return actor;
    }

    private static bool IsAdmin(User user) => user.Role?.Name == "Admin";
    private static bool IsTeacher(User user) => user.Role?.Name == "Teacher";

    private static void EnsureTeacherOrAdmin(User actor)
    {
        if (!IsAdmin(actor) && !IsTeacher(actor))
            throw new SmartExamException(StatusCodes.Status403Forbidden, "Only teachers or admins can perform this action.");
    }

    private async Task EnsureScienceExists(int scienceId)
    {
        var exists = await _context.Sciences.AnyAsync(s => s.Id == scienceId);
        if (!exists)
            throw new SmartExamException(StatusCodes.Status400BadRequest, "Science not found.");
    }

    private async Task EnsureTopicNameUnique(string name, int scienceId, int? excludeId = null)
    {
        var query = _context.Topics.AsQueryable()
            .Where(t => t.ScienceId == scienceId && t.Name == name.Trim());

        if (excludeId.HasValue)
            query = query.Where(t => t.Id != excludeId.Value);

        var exists = await query.AnyAsync();
        if (exists)
            throw new SmartExamException(StatusCodes.Status400BadRequest, "Topic with this name already exists for the given science.");
    }

    private static TopicResponse MapToResponse(Topic topic) =>
        new()
        {
            Id = topic.Id,
            Name = topic.Name,
            Description = topic.Description,
            ScienceId = topic.ScienceId,
            CreatedAt = topic.CreatedAt
        };
}
