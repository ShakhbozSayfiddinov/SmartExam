using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Questions;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class QuestionService(AppDbContext context) : IQuestionService
{
    public async Task<List<QuestionModel>> GetAllAsync()
    {
        var questions = await context.Questions
            .Include(q => q.Topic)
            .Include(q => q.Language)
            .Where(q => !q.IsDeleted)
            .ToListAsync();

        return questions.Select(QuestionModel.MapFromEntity)?.ToList();
    }

    public async Task<QuestionModel> GetByIdAsync(Guid id)
    {
        var question = await context.Questions
            .Include(q => q.Topic)
            .Include(q => q.Language)
            .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);

        return question is null ? null : QuestionModel.MapFromEntity(question);
    }

    public async Task<QuestionModel> CreateAsync(CreateQuestionDto dto)
    {
        var topicExists = await context.Topics.AnyAsync(t => t.Id == dto.TopicId && !t.IsDeleted);
        if (!topicExists)
            throw new SmartExamException(404, "error_topic_not_found");

        var teacherExists = await context.Teachers.AnyAsync(t => t.Id == dto.TeacherId && !t.IsDeleted);
        if (!teacherExists)
            throw new SmartExamException(404, "error_teacher_not_found");

        var languageExists = await context.Languages.AnyAsync(l => l.Id == dto.LanguageId && !l.IsDeleted);
        if (!languageExists)
            throw new SmartExamException(404, "error_language_not_found");

        if (dto.ImageId.HasValue)
        {
            var imageExists = await context.Attachments.AnyAsync(a => a.Id == dto.ImageId && !a.IsDeleted);
            if (!imageExists)
                throw new SmartExamException(404, "error_attachment_not_found");
        }

        var question = new Question
        {
            Title         = dto.Title,
            AnswerA       = dto.AnswerA,
            AnswerB       = dto.AnswerB,
            AnswerC       = dto.AnswerC,
            AnswerD       = dto.AnswerD,
            CorrectAnswer = dto.CorrectAnswer,
            Explanation   = dto.Explanation,
            TopicId       = dto.TopicId,
            TeacherId     = dto.TeacherId,
            LanguageId    = dto.LanguageId,
            ImageId       = dto.ImageId,
        };

        await context.Questions.AddAsync(question);
        await context.SaveChangesAsync();
        await context.Entry(question).Reference(q => q.Topic).LoadAsync();
        await context.Entry(question).Reference(q => q.Language).LoadAsync();

        return QuestionModel.MapFromEntity(question);
    }

    public async Task<QuestionModel> UpdateAsync(Guid id, UpdateQuestionDto dto)
    {
        var question = await context.Questions
            .Include(q => q.Topic)
            .Include(q => q.Language)
            .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted)
            ?? throw new SmartExamException(404, "error_question_not_found");

        var topicExists = await context.Topics.AnyAsync(t => t.Id == dto.TopicId && !t.IsDeleted);
        if (!topicExists)
            throw new SmartExamException(404, "error_topic_not_found");

        var teacherExists = await context.Teachers.AnyAsync(t => t.Id == dto.TeacherId && !t.IsDeleted);
        if (!teacherExists)
            throw new SmartExamException(404, "error_teacher_not_found");

        var languageExists = await context.Languages.AnyAsync(l => l.Id == dto.LanguageId && !l.IsDeleted);
        if (!languageExists)
            throw new SmartExamException(404, "error_language_not_found");

        if (dto.ImageId.HasValue)
        {
            var imageExists = await context.Attachments.AnyAsync(a => a.Id == dto.ImageId && !a.IsDeleted);
            if (!imageExists)
                throw new SmartExamException(404, "error_attachment_not_found");
        }

        question.Title         = dto.Title;
        question.AnswerA       = dto.AnswerA;
        question.AnswerB       = dto.AnswerB;
        question.AnswerC       = dto.AnswerC;
        question.AnswerD       = dto.AnswerD;
        question.CorrectAnswer = dto.CorrectAnswer;
        question.Explanation   = dto.Explanation;
        question.TopicId       = dto.TopicId;
        question.TeacherId     = dto.TeacherId;
        question.LanguageId    = dto.LanguageId;
        question.ImageId       = dto.ImageId;
        question.UpdatedAt     = DateTime.UtcNow;

        await context.SaveChangesAsync();
        await context.Entry(question).Reference(q => q.Topic).LoadAsync();
        await context.Entry(question).Reference(q => q.Language).LoadAsync();

        return QuestionModel.MapFromEntity(question);
    }

    public async Task DeleteAsync(Guid id)
    {
        var question = await context.Questions
            .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted)
            ?? throw new SmartExamException(404, "error_question_not_found");

        question.IsDeleted = true;
        question.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
