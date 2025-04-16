using PANDA.Shared.Common;

namespace PANDA.Api.Models;
public class Patient: AuditableEntity
{ 
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string NHSNumber { get; set; } = default!;
    public string Postcode { get; set; } = default!;
}