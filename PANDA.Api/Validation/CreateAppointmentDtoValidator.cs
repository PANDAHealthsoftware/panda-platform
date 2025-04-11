using FluentValidation;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs.Appointment;

namespace PANDA.Api.Validation;

public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage(ErrorMessages.InvalidPatientId);

        RuleFor(x => x.AppointmentDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(ErrorMessages.FutureAppointmentRequired);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(ErrorMessages.InvalidAppointmentStatus);

        RuleFor(x => x.PatientId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage(ErrorMessages.ClinicianRequired);

        RuleFor(x => x.Department)
            .IsInEnum()
            .WithMessage(ErrorMessages.InvalidDepartment);
    }
}