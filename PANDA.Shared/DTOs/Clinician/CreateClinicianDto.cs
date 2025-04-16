using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs.Clinician;

public class CreateClinicianDto
{
    public string FullName { get; set; } = default!;

    public Department Department { get; set; }

    public string Email { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string? RegistrationNumber { get; set; }
}