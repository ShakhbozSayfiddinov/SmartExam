namespace SmartExam.Application.DTOs.Admin;

public class GetUsersQuery
{
    public string Role { get; set; }
    public string Status { get; set; }
    public string Search { get; set; }
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
}
