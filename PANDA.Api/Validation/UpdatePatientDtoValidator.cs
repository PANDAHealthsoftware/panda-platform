using FluentValidation;
using PANDA.Shared.DTOs.Patient;
using PANDA.Shared.Common;

namespace PANDA.Api.Validation;
public class UpdatePatientDtoValidator : AbstractValidator<UpdatePatientDto>
{
    public UpdatePatientDtoValidator()
    {
        Console.WriteLine("✅ UpdatePatientDtoValidator constructed");

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
    }
}