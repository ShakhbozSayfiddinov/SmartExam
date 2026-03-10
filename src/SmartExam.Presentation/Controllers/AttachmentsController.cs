using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Attachments;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class AttachmentsController(IAttachmentService attachmentService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var attachments = await attachmentService.GetAllAsync();
        return Success(attachments);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var attachment = await attachmentService.GetByIdAsync(id);
        return attachment is null ? NotFound() : Success(attachment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAttachmentDto dto)
    {
        var attachment = await attachmentService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = attachment.Id }, attachment);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAttachmentDto dto)
    {
        var attachment = await attachmentService.UpdateAsync(id, dto);
        return Success(attachment);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await attachmentService.DeleteAsync(id);
        return NoContent();
    }
}
