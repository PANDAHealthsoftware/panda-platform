using PANDA.Domain.Entities;

namespace PANDA.Api.Models;

public class User : AuditableEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;

    public string Status { get; set; } = string.Empty;
    public bool IsStaff { get; set; }
}