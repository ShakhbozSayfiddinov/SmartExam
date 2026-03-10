using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Questions;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class QuestionsController(IQuestionService questionService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var questions = await questionService.GetAllAsync();
        return Success(questions);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var question = await questionService.GetByIdAsync(id);
        return question is null ? NotFound() : Success(question);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuestionDto dto)
    {
        var question = await questionService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = question.Id }, question);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQuestionDto dto)
    {
        var question = await questionService.UpdateAsync(id, dto);
        return Success(question);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await questionService.DeleteAsync(id);
        return NoContent();
    }
}
