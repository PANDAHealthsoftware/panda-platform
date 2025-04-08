using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs
{
    public class UpdateAppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }

        public AppointmentStatus Status { get; set; }
        public string Clinician { get; set; } = string.Empty;
        public Department Department { get; set; }
    }
}