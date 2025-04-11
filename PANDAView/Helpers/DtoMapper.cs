using PANDA.Shared.Common;
using PANDA.Shared.DTOs;

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