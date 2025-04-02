using FluentValidation;
using PANDA.Api.Common;
using PANDA.Api.Dto;

namespace PANDA.Api.Validation;

public class PatientDtoValidator : AbstractValidator<PatientDto>
{
    public PatientDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DateOfBirth)
            .Must(date => date > DateTime.MinValue)
            .WithMessage("Date of birth is required.")
            .LessThan(DateTime.Today)
            .WithMessage("Date of birth must be in the past.");
        RuleFor(x => x.NHSNumber)
            .NotEmpty()
            .Length(10)
            .Must(IsValidNhsNumber)
            .WithMessage("NHS number is invalid.");
        RuleFor(x => x.Postcode).NotEmpty().Matches(@"^[A-Z]{1,2}\d{1,2} ?\d[A-Z]{2}$").WithMessage("Invalid postcode format");
        RuleFor(x => x.Gender)
            .NotNull()
            .WithMessage("Gender is required.")
            .IsInEnum()
            .WithMessage("Gender must be a valid value.");
    }
    
    private bool IsValidNhsNumber(string nhsNumber)
    {
        if (!long.TryParse(nhsNumber, out _) || nhsNumber.Length != 10)
            return false;

        var digits = nhsNumber.Select(c => int.Parse(c.ToString())).ToArray();
        var total = 0;
        for (int i = 0; i < 9; i++)
            total += digits[i] * (10 - i);

        var remainder = total % 11;
        var checkDigit = 11 - remainder;
        if (checkDigit == 11) checkDigit = 0;
        if (checkDigit == 10) return false;

        return checkDigit == digits[9];
    }
}