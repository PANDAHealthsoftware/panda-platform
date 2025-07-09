namespace PANDA.Shared.DTOs.User;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public int RoleId { get; set; }   // ✅ correct single FK

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsStaff { get; set; } = false;
}