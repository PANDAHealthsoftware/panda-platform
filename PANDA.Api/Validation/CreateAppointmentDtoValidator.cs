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
            .WithMessage(ErrorMessages.InvalidPatientId);

        RuleFor(x => x.AppointmentDate)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage(ErrorMessages.FutureAppointmentRequired);

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