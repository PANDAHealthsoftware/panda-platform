using PANDA.Shared.DTOs.Clinician;

namespace PANDAView.Helpers
{
    public static class ClinicianDtoExtensions
    {
        public static CreateClinicianDto ToCreateDto(this ClinicianDto dto)
        {
            return new CreateClinicianDto
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Department = dto.Department,
                RegistrationNumber = dto.RegistrationNumber,
                IsActive = dto.IsActive
            };
        }

        public static UpdateClinicianDto ToUpdateDto(this ClinicianDto dto)
        {
            return new UpdateClinicianDto
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Department = dto.Department,
                RegistrationNumber = dto.RegistrationNumber,
                IsActive = dto.IsActive
            };
        }
    }
}