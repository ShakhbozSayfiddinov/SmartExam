using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Students;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class StudentsController(IStudentService studentService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await studentService.GetAllAsync();
        return Success(students);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var student = await studentService.GetByIdAsync(id);
        return student is null ? NotFound() : Success(student);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
    {
        var student = await studentService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = student.Id }, student);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await studentService.DeleteAsync(id);
        return NoContent();
    }
}
