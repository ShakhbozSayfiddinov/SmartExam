using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Departments;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class DepartmentService(AppDbContext context) : IDepartmentService
{
    public async Task<List<DepartmentModel>> GetAllAsync()
    {
        var departments = await context.Departments
            .Include(d => d.Science)
            .Where(d => !d.IsDeleted)
            .ToListAsync();

        return departments.Select(DepartmentModel.MapFromEntity)?.ToList();
    }

    public async Task<DepartmentModel> GetByIdAsync(Guid id)
    {
        var department = await context.Departments
            .Include(d => d.Science)
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

        return department is null ? null : DepartmentModel.MapFromEntity(department);
    }

    public async Task<DepartmentModel> CreateAsync(CreateDepartmentDto dto)
    {
        var scienceExists = await context.Sciences.AnyAsync(s => s.Id == dto.ScienceId && !s.IsDeleted);
        if (!scienceExists)
            throw new SmartExamException(404, "error_science_not_found");

        var department = new Department
        {
            Name        = dto.Name,
            Description = dto.Description,
            ScienceId   = dto.ScienceId,
        };

        await context.Departments.AddAsync(department);
        await context.SaveChangesAsync();
        await context.Entry(department).Reference(d => d.Science).LoadAsync();

        return DepartmentModel.MapFromEntity(department);
    }

    public async Task<DepartmentModel> UpdateAsync(Guid id, UpdateDepartmentDto dto)
    {
        var department = await context.Departments
            .Include(d => d.Science)
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted)
            ?? throw new SmartExamException(404, "error_department_not_found");

        var scienceExists = await context.Sciences.AnyAsync(s => s.Id == dto.ScienceId && !s.IsDeleted);
        if (!scienceExists)
            throw new SmartExamException(404, "error_science_not_found");

        department.Name        = dto.Name;
        department.Description = dto.Description;
        department.ScienceId   = dto.ScienceId;
        department.UpdatedAt   = DateTime.UtcNow;

        await context.SaveChangesAsync();
        await context.Entry(department).Reference(d => d.Science).LoadAsync();

        return DepartmentModel.MapFromEntity(department);
    }

    public async Task DeleteAsync(Guid id)
    {
        var department = await context.Departments
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted)
            ?? throw new SmartExamException(404, "error_department_not_found");

        department.IsDeleted = true;
        department.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
