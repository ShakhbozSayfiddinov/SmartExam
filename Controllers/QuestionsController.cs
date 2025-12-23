using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartExam.Exceptions;
using SmartExam.Extensions;
using SmartExam.Models.Questions;
using SmartExam.Services.Interfaces;

namespace SmartExam.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var actorUserId = GetActorId();
            var questions = await _questionService.GetAllAsync(actorUserId);
            return ResponseHandler.ReturnResponseList(questions);
        }
        catch (SmartExamException ex)
        {
            return ResponseHandler.ReturnError(ex.Message, ex.StatusCode);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var actorUserId = GetActorId();
            var question = await _questionService.GetByIdAsync(id, actorUserId);
            return ResponseHandler.ReturnIActionResponse(question);
        }
        catch (SmartExamException ex)
        {
            return ResponseHandler.ReturnError(ex.Message, ex.StatusCode);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] QuestionCreateRequest request)
    {
        try
        {
            var actorUserId = GetActorId();
            var question = await _questionService.CreateAsync(request, actorUserId);
            return ResponseHandler.ReturnIActionResponse(question);
        }
        catch (SmartExamException ex)
        {
            return ResponseHandler.ReturnError(ex.Message, ex.StatusCode);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] QuestionUpdateRequest request)
    {
        try
        {
            var actorUserId = GetActorId();
            var question = await _questionService.UpdateAsync(id, request, actorUserId);
            return ResponseHandler.ReturnIActionResponse(question);
        }
        catch (SmartExamException ex)
        {
            return ResponseHandler.ReturnError(ex.Message, ex.StatusCode);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var actorUserId = GetActorId();
            await _questionService.DeleteAsync(id, actorUserId);
            return ResponseHandler.ReturnIActionResponse("Question deleted.");
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
