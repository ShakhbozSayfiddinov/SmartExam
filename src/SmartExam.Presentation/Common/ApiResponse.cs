namespace SmartExam.Presentation.Common;

public class ApiResponse<T>(T data)
{
    public bool Success { get; init; } = true;
    public T Data { get; init; } = data;
}
