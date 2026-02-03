namespace  EventHub.Api.Entities;
public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHashed { get; set; } = string.Empty;
    public string Role { get; set; } = "User";

    
}
