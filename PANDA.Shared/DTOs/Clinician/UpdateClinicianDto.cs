namespace PANDA.Shared.DTOs.Clinician;

public class UpdateClinicianDto: CreateClinicianDto
{
    public int Id { get; set; }

    public bool IsActive { get; set; }
}