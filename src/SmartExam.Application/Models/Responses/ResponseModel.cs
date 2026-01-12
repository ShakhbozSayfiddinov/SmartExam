namespace SmartExam.Models.Responses;

public class ResponseModel<T>
{
    public bool Status { get; set; }
    public T Data { get; set; }
    public string Error { get; set; }
}
