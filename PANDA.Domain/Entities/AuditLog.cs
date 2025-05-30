namespace PANDA.Domain.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Changes { get; set; } = string.Empty; // JSON diff or summary
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}