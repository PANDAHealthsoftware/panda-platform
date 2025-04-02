using PANDA.Api.Common;

namespace PANDA.Api.Dto
{
    public class AppointmentDto
    {
        public int Id { get; init; }
        public int PatientId { get; init; }
        public DateTimeOffset AppointmentDate { get; init; }
        public AppointmentStatus Status { get; init; }
        public string Clinician { get; init; } = string.Empty;
        public string Department { get; init; } = string.Empty;
    }
}