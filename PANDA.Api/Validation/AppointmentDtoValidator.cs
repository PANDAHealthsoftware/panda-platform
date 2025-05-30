using FluentValidation;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Appointment;

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

        RuleFor(x => x.ClinicianId)
            .GreaterThan(0)
            .WithMessage(ErrorMessages.ClinicianRequired);

        RuleFor(x => x.Department)
            .IsInEnum()
            .WithMessage(ErrorMessages.InvalidDepartment);
    }
}