using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Languages;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class LanguagesController(ILanguageService languageService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var languages = await languageService.GetAllAsync();
        return Success(languages);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var language = await languageService.GetByIdAsync(id);
        return language is null ? NotFound() : Success(language);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLanguageDto dto)
    {
        var language = await languageService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = language.Id }, language);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLanguageDto dto)
    {
        var language = await languageService.UpdateAsync(id, dto);
        return Success(language);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await languageService.DeleteAsync(id);
        return NoContent();
    }
}
