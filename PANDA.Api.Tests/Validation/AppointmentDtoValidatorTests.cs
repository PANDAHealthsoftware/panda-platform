using FluentValidation.TestHelper;
using PANDA.Api.Dto;
using PANDA.Api.Validation;
using PANDA.Shared.Enums;

namespace PANDA.Api.Tests.Validation;

public class CreateAppointmentDtoValidatorTests
{
    private readonly CreateAppointmentDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Required_Fields_Are_Missing()
    {
        var dto = new CreateAppointmentDto();

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.PatientId);
        result.ShouldHaveValidationErrorFor(x => x.AppointmentDate);
        result.ShouldHaveValidationErrorFor(x => x.Clinician);
        result.ShouldHaveValidationErrorFor(x => x.Department);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Dto()
    {
        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Scheduled,
            Clinician = "Dr. Valid",
            Department = "ValidDept"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }
}