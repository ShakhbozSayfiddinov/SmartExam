using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartExam.Exceptions;
using SmartExam.Extensions;
using SmartExam.Models.Topics;
using SmartExam.Services.Interfaces;

namespace SmartExam.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController : ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicsController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var actorUserId = GetActorId();
            var topics = await _topicService.GetAllAsync(actorUserId);
            return ResponseHandler.ReturnResponseList(topics);
        }
        catch (SmartExamException ex)
        {
            return ResponseHandler.ReturnError(ex.Message, ex.StatusCode);
        }
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var actorUserId = GetActorId();
            var topic = await _topicService.GetByIdAsync(id, actorUserId);
            return ResponseHandler.ReturnIActionResponse(topic);
        }
        catch (SmartExamException ex)
        {
            return ResponseHandler.ReturnError(ex.Message, ex.StatusCode);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] TopicCreateRequest request)
    {
        try
        {
            var actorUserId = GetActorId();
            var topic = await _topicService.CreateAsync(request, actorUserId);
            return ResponseHandler.ReturnIActionResponse(topic);
        }
        catch (SmartExamException ex)
        {
            return ResponseHandler.ReturnError(ex.Message, ex.StatusCode);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] TopicUpdateRequest request)
    {
        try
        {
            var actorUserId = GetActorId();
            var topic = await _topicService.UpdateAsync(id, request, actorUserId);
            return ResponseHandler.ReturnIActionResponse(topic);
        }
        catch (SmartExamException ex)
        {
            return ResponseHandler.ReturnError(ex.Message, ex.StatusCode);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var actorUserId = GetActorId();
            await _topicService.DeleteAsync(id, actorUserId);
            return ResponseHandler.ReturnIActionResponse("Topic deleted.");
        }
        catch (SmartExamException ex)
        {
            return ResponseHandler.ReturnError(ex.Message, ex.StatusCode);
        }
    }

    private int GetActorId()
    {
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(idValue) || !int.TryParse(idValue, out var userId))
        {
            throw new SmartExamException(StatusCodes.Status401Unauthorized, "Invalid or missing token.");
        }

        return userId;
    }
}
