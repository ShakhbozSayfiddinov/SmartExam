using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Students;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class StudentService(AppDbContext context) : IStudentService
{
    public async Task<List<StudentModel>> GetAllAsync()
    {
        var students = await context.Students
            .Include(s => s.User)
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        return students.Select(StudentModel.MapFromEntity)?.ToList();
    }

    public async Task<StudentModel> GetByIdAsync(Guid id)
    {
        var student = await context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        return student is null ? null : StudentModel.MapFromEntity(student);
    }

    public async Task<StudentModel> CreateAsync(CreateStudentDto dto)
    {
        var userExists = await context.Users.AnyAsync(u => u.Id == dto.UserId && !u.IsDeleted);
        if (!userExists)
            throw new SmartExamException(404, "error_user_not_found");

        var alreadyStudent = await context.Students.AnyAsync(s => s.UserId == dto.UserId && !s.IsDeleted);
        if (alreadyStudent)
            throw new SmartExamException(409, "error_user_already_student");

        var student = new Student { UserId = dto.UserId };

        await context.Students.AddAsync(student);
        await context.SaveChangesAsync();
        await context.Entry(student).Reference(s => s.User).LoadAsync();

        return StudentModel.MapFromEntity(student);
    }

    public async Task DeleteAsync(Guid id)
    {
        var student = await context.Students
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted)
            ?? throw new SmartExamException(404, "error_student_not_found");

        student.IsDeleted = true;
        student.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
