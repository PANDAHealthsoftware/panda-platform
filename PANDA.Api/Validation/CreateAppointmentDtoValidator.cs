using FluentValidation;
using PANDA.Api.Common;
using PANDA.Api.Dto;

namespace PANDA.Api.Validation;

public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage("PatientId is required.");

        RuleFor(x => x.AppointmentDate)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("Appointment must be scheduled in the future.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(ErrorMessages.InvalidAppointmentStatus);

        RuleFor(x => x.Clinician)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Clinician name is required and must be 100 characters or fewer.");

        RuleFor(x => x.Department)
            .IsInEnum()
            .WithMessage("Invalid department.");
    }
}