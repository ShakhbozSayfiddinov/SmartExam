using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Sciences;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class SciencesController(IScienceService scienceService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sciences = await scienceService.GetAllAsync();
        return Success(sciences);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var science = await scienceService.GetByIdAsync(id);
        return science is null ? NotFound() : Success(science);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateScienceDto dto)
    {
        var science = await scienceService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = science.Id }, science);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateScienceDto dto)
    {
        var science = await scienceService.UpdateAsync(id, dto);
        return Success(science);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await scienceService.DeleteAsync(id);
        return NoContent();
    }
}
