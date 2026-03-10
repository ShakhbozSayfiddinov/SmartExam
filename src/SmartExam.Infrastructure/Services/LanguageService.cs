using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Languages;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class LanguageService(AppDbContext context) : ILanguageService
{
    public async Task<List<LanguageModel>> GetAllAsync()
    {
        var languages = await context.Languages
            .Where(l => !l.IsDeleted)
            .ToListAsync();

        return languages.Select(LanguageModel.MapFromEntity)?.ToList();
    }

    public async Task<LanguageModel> GetByIdAsync(Guid id)
    {
        var language = await context.Languages
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);

        return language is null ? null : LanguageModel.MapFromEntity(language);
    }

    public async Task<LanguageModel> CreateAsync(CreateLanguageDto dto)
    {
        var language = new Language
        {
            Name = dto.Name,
            Code = dto.Code,
        };

        await context.Languages.AddAsync(language);
        await context.SaveChangesAsync();

        return LanguageModel.MapFromEntity(language);
    }

    public async Task<LanguageModel> UpdateAsync(Guid id, UpdateLanguageDto dto)
    {
        var language = await context.Languages
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted)
            ?? throw new SmartExamException(404, "error_language_not_found");

        language.Name      = dto.Name;
        language.Code      = dto.Code;
        language.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return LanguageModel.MapFromEntity(language);
    }

    public async Task DeleteAsync(Guid id)
    {
        var language = await context.Languages
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted)
            ?? throw new SmartExamException(404, "error_language_not_found");

        language.IsDeleted = true;
        language.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
