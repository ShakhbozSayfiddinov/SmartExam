namespace SmartExam.Domain.Exceptions;

public class SmartExamException(int statusCode, string errorCode) : Exception(errorCode)
{
    public int StatusCode { get; } = statusCode;
    public string ErrorCode { get; } = errorCode;
}
