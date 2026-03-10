namespace SmartExam.Application.Models;

public class AuthModel
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}
