using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs;

public class CreateClinicianDto
{
    public string Name { get; set; } = default!;
    public Department Department { get; set; }
}