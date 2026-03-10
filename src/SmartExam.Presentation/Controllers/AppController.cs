using Microsoft.AspNetCore.Mvc;
using SmartExam.Presentation.Common;

namespace SmartExam.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class AppController : ControllerBase
{
    protected IActionResult Success<T>(T data)
        => Ok(new ApiResponse<T>(data));

    protected IActionResult Created<T>(string actionName, object routeValues, T data)
        => CreatedAtAction(actionName, routeValues, new ApiResponse<T>(data));
}
