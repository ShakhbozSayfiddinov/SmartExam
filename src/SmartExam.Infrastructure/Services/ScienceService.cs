using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Sciences;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class ScienceService(AppDbContext context) : IScienceService
{
    public async Task<List<ScienceModel>> GetAllAsync()
    {
        var sciences = await context.Sciences
            .Include(s => s.ParentScience)
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        return sciences.Select(ScienceModel.MapFromEntity)?.ToList();
    }

    public async Task<ScienceModel> GetByIdAsync(Guid id)
    {
        var science = await context.Sciences
            .Include(s => s.ParentScience)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        return science is null ? null : ScienceModel.MapFromEntity(science);
    }

    public async Task<ScienceModel> CreateAsync(CreateScienceDto dto)
    {
        if (dto.ScienceId.HasValue)
        {
            var parentExists = await context.Sciences.AnyAsync(s => s.Id == dto.ScienceId && !s.IsDeleted);
            if (!parentExists)
                throw new SmartExamException(404, "error_parent_science_not_found");
        }

        var science = new Science
        {
            Name        = dto.Name,
            Description = dto.Description,
            ScienceId   = dto.ScienceId,
        };

        await context.Sciences.AddAsync(science);
        await context.SaveChangesAsync();
        await context.Entry(science).Reference(s => s.ParentScience).LoadAsync();

        return ScienceModel.MapFromEntity(science);
    }

    public async Task<ScienceModel> UpdateAsync(Guid id, UpdateScienceDto dto)
    {
        var science = await context.Sciences
            .Include(s => s.ParentScience)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted)
            ?? throw new SmartExamException(404, "error_science_not_found");

        if (dto.ScienceId.HasValue)
        {
            if (dto.ScienceId == id)
                throw new SmartExamException(400, "error_science_cannot_be_own_parent");

            var parentExists = await context.Sciences.AnyAsync(s => s.Id == dto.ScienceId && !s.IsDeleted);
            if (!parentExists)
                throw new SmartExamException(404, "error_parent_science_not_found");
        }

        science.Name        = dto.Name;
        science.Description = dto.Description;
        science.ScienceId   = dto.ScienceId;
        science.UpdatedAt   = DateTime.UtcNow;

        await context.SaveChangesAsync();
        await context.Entry(science).Reference(s => s.ParentScience).LoadAsync();

        return ScienceModel.MapFromEntity(science);
    }

    public async Task DeleteAsync(Guid id)
    {
        var science = await context.Sciences
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted)
            ?? throw new SmartExamException(404, "error_science_not_found");

        science.IsDeleted = true;
        science.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
