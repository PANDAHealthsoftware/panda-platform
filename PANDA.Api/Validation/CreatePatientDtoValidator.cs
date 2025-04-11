using FluentValidation;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Patient;

namespace PANDA.Api.Validation;

public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage(ErrorMessages.NameRequired);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage(ErrorMessages.NameRequired);

        RuleFor(x => x.DateOfBirth)
            .MustBeValidDateOfBirth();

        RuleFor(x => x.NHSNumber)
            .MustBeValidNhsNumber();

        RuleFor(x => x.Postcode)
            .MustBeValidPostcode();

        RuleFor(x => x.Gender)
            .NotNull()
            .WithMessage("Gender is required.")
            .IsInEnum()
            .WithMessage(ErrorMessages.InvalidGender);
    }
}