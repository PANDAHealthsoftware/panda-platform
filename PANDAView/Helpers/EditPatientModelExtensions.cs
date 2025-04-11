using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Patient;
using PANDAView.Models.Patient;

namespace PANDAView.Helpers
{
    public static class EditPatientModelExtensions
    {
        public static CreatePatientDto ToCreateDto(this EditPatientModel model) => new()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            NHSNumber = model.NHSNumber,
            DateOfBirth = model.DateOfBirth
        };

        public static UpdatePatientDto ToUpdateDto(this EditPatientModel model) => new()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            NHSNumber = model.NHSNumber,
            DateOfBirth = model.DateOfBirth
        };
    }
}