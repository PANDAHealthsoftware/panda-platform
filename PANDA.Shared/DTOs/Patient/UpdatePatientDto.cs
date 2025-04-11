using PANDA.Shared.Common;

namespace PANDA.Shared.DTOs.Patient;

public class UpdatePatientDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string NHSNumber { get; set; } = default!;
    public string Postcode { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
}