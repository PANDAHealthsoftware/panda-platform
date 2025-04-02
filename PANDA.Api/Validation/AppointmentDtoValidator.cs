using FluentValidation;
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
            .WithMessage("Invalid appointment status.");

        RuleFor(x => x.Clinician)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Clinician name is required and must be 100 characters or fewer.");

        RuleFor(x => x.Department)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Department is required and must be 100 characters or fewer.");
    }
}