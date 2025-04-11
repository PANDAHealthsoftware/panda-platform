using FluentValidation;
using PANDA.Shared.Common;

namespace PANDA.Api.Validation;

public static class ValidationRules
{
    public static IRuleBuilderOptions<T, int> MustBeValidPatientId<T>(this IRuleBuilder<T, int> ruleBuilder) =>
        ruleBuilder
            .GreaterThan(0)
            .WithMessage(ErrorMessages.InvalidPatientId);

    public static IRuleBuilderOptions<T, DateTime> MustBeValidDateOfBirth<T>(this IRuleBuilder<T, DateTime> ruleBuilder) =>
        ruleBuilder
            .Must(date => date > DateTime.MinValue)
            .WithMessage(ErrorMessages.DateOfBirthRequired)
            .LessThan(DateTime.Today)
            .WithMessage(ErrorMessages.InvalidDateOfBirth);

    public static IRuleBuilderOptions<T, string> MustBeValidNhsNumber<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty()
            .Length(10)
            .WithMessage(ErrorMessages.InvalidNhsNumber)
            .Must(IsValidNhsNumber)
            .WithMessage(ErrorMessages.InvalidNhsChecksum);

    public static IRuleBuilderOptions<T, string> MustBeValidPostcode<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty()
            .Matches(@"^[A-Z]{1,2}\d{1,2} ?\d[A-Z]{2}$")
            .WithMessage(ErrorMessages.InvalidPostcodeFormat);

    private static bool IsValidNhsNumber(string nhsNumber)
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