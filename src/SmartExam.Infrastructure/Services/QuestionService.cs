using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartExam.Data;
using SmartExam.Entities;
using SmartExam.Exceptions;
using SmartExam.Models.Questions;
using SmartExam.Services.Interfaces;

namespace SmartExam.Services;

public class QuestionService : IQuestionService
{
    private readonly ApplicationDbContext _context;

    public QuestionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<QuestionResponse> CreateAsync(QuestionCreateRequest request, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        EnsureTeacherOrAdmin(actor);

        await EnsureTopicExists(request.TopicId);

        var question = new Question
        {
            Title = request.Title.Trim(),
            AnswerA = request.AnswerA.Trim(),
            AnswerB = request.AnswerB.Trim(),
            AnswerC = request.AnswerC.Trim(),
            AnswerD = request.AnswerD.Trim(),
            CorrectAnswer = ParseCorrectAnswer(request.CorrectAnswer),
            Explation = request.Explanation?.Trim(),
            TopicId = request.TopicId,
            CreatedByUserId = actor.Id
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        return MapToResponse(question);
    }

    public async Task<IEnumerable<QuestionResponse>> GetAllAsync(int actorUserId)
    {
        await GetActorAsync(actorUserId);

        var questions = await _context.Questions.AsNoTracking().ToListAsync();
        return questions.Select(MapToResponse);
    }

    public async Task<QuestionResponse> GetByIdAsync(int id, int actorUserId)
    {
        await GetActorAsync(actorUserId);

        var question = await _context.Questions.AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id)
            ?? throw new SmartExamException(StatusCodes.Status404NotFound, "Question not found.");

        return MapToResponse(question);
    }

    public async Task<QuestionResponse> UpdateAsync(int id, QuestionUpdateRequest request, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id)
            ?? throw new SmartExamException(StatusCodes.Status404NotFound, "Question not found.");

        EnsureCanModify(actor, question);

        if (!string.IsNullOrWhiteSpace(request.Title))
            question.Title = request.Title.Trim();

        if (!string.IsNullOrWhiteSpace(request.AnswerA))
            question.AnswerA = request.AnswerA.Trim();
        if (!string.IsNullOrWhiteSpace(request.AnswerB))
            question.AnswerB = request.AnswerB.Trim();
        if (!string.IsNullOrWhiteSpace(request.AnswerC))
            question.AnswerC = request.AnswerC.Trim();
        if (!string.IsNullOrWhiteSpace(request.AnswerD))
            question.AnswerD = request.AnswerD.Trim();

        if (!string.IsNullOrWhiteSpace(request.CorrectAnswer))
            question.CorrectAnswer = ParseCorrectAnswer(request.CorrectAnswer);

        if (request.Explanation is not null)
            question.Explation = request.Explanation.Trim();

        if (request.TopicId.HasValue && request.TopicId.Value != question.TopicId)
        {
            await EnsureTopicExists(request.TopicId.Value);
            question.TopicId = request.TopicId.Value;
        }

        question.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapToResponse(question);
    }

    public async Task DeleteAsync(int id, int actorUserId)
    {
        var actor = await GetActorAsync(actorUserId);
        var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id)
            ?? throw new SmartExamException(StatusCodes.Status404NotFound, "Question not found.");

        EnsureCanModify(actor, question);

        question.IsDeleted = true;
        question.UpdatedAt = DateTime.UtcNow;
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

    private static void EnsureCanModify(User actor, Question question)
    {
        if (IsAdmin(actor))
            return;

        if (IsTeacher(actor) && question.CreatedByUserId == actor.Id)
            return;

        throw new SmartExamException(StatusCodes.Status403Forbidden, "Not allowed to modify this question.");
    }

    private async Task EnsureTopicExists(int topicId)
    {
        var exists = await _context.Topics.AnyAsync(t => t.Id == topicId);
        if (!exists)
            throw new SmartExamException(StatusCodes.Status400BadRequest, "Topic not found.");
    }

    private static char ParseCorrectAnswer(string correctAnswer)
    {
        if (string.IsNullOrWhiteSpace(correctAnswer))
            throw new SmartExamException(StatusCodes.Status400BadRequest, "CorrectAnswer is required.");

        var value = char.ToUpperInvariant(correctAnswer.Trim()[0]);
        if (value is not ('A' or 'B' or 'C' or 'D'))
            throw new SmartExamException(StatusCodes.Status400BadRequest, "CorrectAnswer must be one of: A, B, C, D.");

        return value;
    }

    private static QuestionResponse MapToResponse(Question question) =>
        new()
        {
            Id = question.Id,
            Title = question.Title,
            AnswerA = question.AnswerA,
            AnswerB = question.AnswerB,
            AnswerC = question.AnswerC,
            AnswerD = question.AnswerD,
            CorrectAnswer = question.CorrectAnswer,
            Explanation = question.Explation,
            TopicId = question.TopicId,
            CreatedByUserId = question.CreatedByUserId,
            CreatedAt = question.CreatedAt
        };
}
