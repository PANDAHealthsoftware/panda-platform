namespace PANDA.Domain.Entities;

public class AuditableEntity
{
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public DateTime? LastModified { get; set; }
}