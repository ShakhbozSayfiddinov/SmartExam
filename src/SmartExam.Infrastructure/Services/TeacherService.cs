using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Teachers;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class TeacherService(AppDbContext context) : ITeacherService
{
    public async Task<List<TeacherModel>> GetAllAsync()
    {
        var teachers = await context.Teachers
            .Include(t => t.User)
            .Where(t => !t.IsDeleted)
            .ToListAsync();

        return teachers.Select(TeacherModel.MapFromEntity)?.ToList();
    }

    public async Task<TeacherModel> GetByIdAsync(Guid id)
    {
        var teacher = await context.Teachers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        return teacher is null ? null : TeacherModel.MapFromEntity(teacher);
    }

    public async Task<TeacherModel> CreateAsync(CreateTeacherDto dto)
    {
        var userExists = await context.Users.AnyAsync(u => u.Id == dto.UserId && !u.IsDeleted);
        if (!userExists)
            throw new SmartExamException(404, "error_user_not_found");

        var alreadyTeacher = await context.Teachers.AnyAsync(t => t.UserId == dto.UserId && !t.IsDeleted);
        if (alreadyTeacher)
            throw new SmartExamException(409, "error_user_already_teacher");

        var teacher = new Teacher { UserId = dto.UserId };

        await context.Teachers.AddAsync(teacher);
        await context.SaveChangesAsync();
        await context.Entry(teacher).Reference(t => t.User).LoadAsync();

        return TeacherModel.MapFromEntity(teacher);
    }

    public async Task DeleteAsync(Guid id)
    {
        var teacher = await context.Teachers
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
            ?? throw new SmartExamException(404, "error_teacher_not_found");

        teacher.IsDeleted = true;
        teacher.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
