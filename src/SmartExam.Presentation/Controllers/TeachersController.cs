using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Teachers;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class TeachersController(ITeacherService teacherService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var teachers = await teacherService.GetAllAsync();
        return Success(teachers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var teacher = await teacherService.GetByIdAsync(id);
        return teacher is null ? NotFound() : Success(teacher);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeacherDto dto)
    {
        var teacher = await teacherService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = teacher.Id }, teacher);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await teacherService.DeleteAsync(id);
        return NoContent();
    }
}
