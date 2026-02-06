namespace SmartExam.Models.Users;

public class UserUpdateRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public int? RoleId { get; set; } // only admin can change
}
