using FluentValidation.TestHelper;
using PANDA.Api.Validation;
using PANDA.Shared.DTOs.Appointment;
using PANDA.Shared.Enums;

namespace PANDA.Api.Tests.Validation;

public class CreateAppointmentDtoValidatorTests
{
    private readonly CreateAppointmentDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Clinician_Is_Missing()
    {
        var validator = new CreateAppointmentDtoValidator();

        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Department = Department.Cardiology 
        };

        var result = validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ClinicianId);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Dto()
    {
        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            ClinicianId = 2, 
            AppointmentDate = DateTime.UtcNow.AddHours(1),
            Status = AppointmentStatus.Scheduled,
            Department = Department.Cardiology
        };

        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}