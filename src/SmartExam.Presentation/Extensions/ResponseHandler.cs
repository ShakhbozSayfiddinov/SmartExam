using Microsoft.AspNetCore.Mvc;
using SmartExam.Models.Responses;

namespace SmartExam.Extensions;

public static class ResponseHandler
{
    public static ActionResult<T> ReturnActionResponse<T>(T model)
    {
        return new OkObjectResult(new ResponseModel<T>
        {
            Status = true,
            Data = model
        });
    }

    public static IActionResult ReturnIActionResponse<T>(T model)
    {
        return new OkObjectResult(new ResponseModel<T>
        {
            Status = true,
            Data = model
        });
    }

    public static IActionResult ReturnResponseList<T>(IEnumerable<T> model)
    {
        return new OkObjectResult(new ResponseModel<IEnumerable<T>>
        {
            Status = true,
            Data = model
        });
    }

    public static ObjectResult ReturnError(string message, int statusCode)
    {
        return new ObjectResult(new ResponseModel<string>
        {
            Status = false,
            Error = message
        })
        {
            StatusCode = statusCode
        };
    }
}
