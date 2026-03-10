using SmartExam.Domain.Exceptions;

namespace SmartExam.Presentation.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (SmartExamException ex)
        {
            logger.LogWarning(ex, "SmartExamException [{StatusCode}]: {ErrorCode}", ex.StatusCode, ex.ErrorCode);
            await WriteErrorAsync(context, ex.StatusCode, ex.ErrorCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteErrorAsync(context, 500, "error_internal_server_error");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string errorCode)
    {
        context.Response.StatusCode  = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { errorCode });
    }
}
