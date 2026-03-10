using Microsoft.AspNetCore.Mvc;
using SmartExam.Application.DTOs.Topics;
using SmartExam.Application.Interfaces;

namespace SmartExam.Presentation.Controllers;

public class TopicsController(ITopicService topicService) : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var topics = await topicService.GetAllAsync();
        return Success(topics);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var topic = await topicService.GetByIdAsync(id);
        return topic is null ? NotFound() : Success(topic);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTopicDto dto)
    {
        var topic = await topicService.CreateAsync(dto);
        return Created(nameof(GetById), new { id = topic.Id }, topic);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTopicDto dto)
    {
        var topic = await topicService.UpdateAsync(id, dto);
        return Success(topic);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await topicService.DeleteAsync(id);
        return NoContent();
    }
}
