using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs.Clinician;

public class ClinicianDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    public Department Department { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string? RegistrationNumber { get; set; } // e.g. GMC/NMC number

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}