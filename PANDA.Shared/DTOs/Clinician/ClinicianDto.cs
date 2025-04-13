using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs.Clinician;

public class ClinicianDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public Department Department { get; set; }
}