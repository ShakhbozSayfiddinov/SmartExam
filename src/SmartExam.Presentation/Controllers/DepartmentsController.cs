using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Departments;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class DepartmentsController(IDepartmentService departmentService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departments = await departmentService.GetAllAsync();
        return Success(departments);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var department = await departmentService.GetByIdAsync(id);
        return department is null ? NotFound() : Success(department);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        var department = await departmentService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = department.Id }, department);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentDto dto)
    {
        var department = await departmentService.UpdateAsync(id, dto);
        return Success(department);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await departmentService.DeleteAsync(id);
        return NoContent();
    }
}
