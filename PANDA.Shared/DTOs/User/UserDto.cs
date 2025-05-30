namespace PANDA.Shared.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;

    // For backward compatibility (single-role systems)
    public string Role { get; set; } = string.Empty;

    // For multi-role systems (recommended going forward)
    public List<string> Roles { get; set; } = new();

    // Do NOT expose PasswordHash in a DTO returned to the client!
    // Remove this in new code. Use only for internal calls if needed.
    public string? PasswordHash { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string Status { get; set; } = string.Empty;
}