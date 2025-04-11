using PANDA.Shared.Common;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Patient;

namespace PANDAView.Helpers
{
    public static class DtoMapper
    {
        public static CreatePatientDto ToCreateDto(this PatientDto patient) => new()
        {
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            NHSNumber = patient.NHSNumber,
            Postcode = patient.Postcode,
            Gender = patient.Gender ?? Gender.Unknown
        };
    }
}