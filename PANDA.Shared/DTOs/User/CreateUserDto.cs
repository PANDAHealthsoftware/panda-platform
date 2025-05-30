namespace PANDA.Shared.DTOs.User;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;   // Plain password for creation only!
    public List<int> RoleIds { get; set; } = new();        // Multi-role support
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string Status { get; set; } = string.Empty;
}