namespace SmartExam.Exceptions;

public class SmartExamException : Exception
{
    public int StatusCode { get; }

    public SmartExamException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}
