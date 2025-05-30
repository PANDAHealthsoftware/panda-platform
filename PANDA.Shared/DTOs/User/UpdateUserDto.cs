namespace PANDA.Shared.DTOs.User;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<int> RoleIds { get; set; } = new();
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string Status { get; set; } = string.Empty;
}