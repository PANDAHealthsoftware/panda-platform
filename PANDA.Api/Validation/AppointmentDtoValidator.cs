using FluentValidation;
using PANDA.Api.Common;
using PANDA.Api.Dto;
using PANDA.Shared.DTOs;

namespace PANDA.Api.Validation;

public class AppointmentDtoValidator : AbstractValidator<UpdateAppointmentDto>
{
    public AppointmentDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage(ErrorMessages.InvalidPatientId);

        RuleFor(x => x.AppointmentDate)
            .GreaterThan(DateTime.UtcNow)
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