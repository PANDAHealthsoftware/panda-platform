using FluentValidation;
using PANDA.Api.Dto;

namespace PANDA.Api.Validation
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(x => x.PatientId).GreaterThan(0).WithMessage("PatientId is required.");
            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTimeOffset.UtcNow)
                .WithMessage("Appointment must be scheduled in the future.");
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid appointment status.");
            RuleFor(x => x.Clinician)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.Department)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}