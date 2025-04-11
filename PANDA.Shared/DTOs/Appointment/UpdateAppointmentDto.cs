using System.ComponentModel.DataAnnotations;
using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs.Appointment
{
    public class UpdateAppointmentDto
    {
        [Required(ErrorMessage = "A valid patient ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "PatientId must be greater than 0.")]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }

        public AppointmentStatus Status { get; set; }
        
        public int ClinicianId { get; set; }
        public string Clinician { get; set; } = string.Empty;
        public Department Department { get; set; }
    }
}