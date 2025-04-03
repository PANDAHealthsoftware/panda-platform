using System.Runtime.InteropServices.JavaScript;
using FluentValidation;
using PANDA.Api.Common;
using PANDA.Api.Dto;

namespace PANDA.Api.Validation;

public class AppointmentDtoValidator : AbstractValidator<AppointmentDto>
{
    public AppointmentDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage("A valid patient ID is required.");

        RuleFor(x => x.AppointmentDate)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("Appointment date must be in the future.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(ErrorMessages.InvalidAppointmentStatus);

        RuleFor(x => x.Clinician)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage(ErrorMessages.ClinicianRequired);

        RuleFor(x => x.Department)
            .IsInEnum()
            .WithMessage(ErrorMessages.InvalidDepartment);
    }
}