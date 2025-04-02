using FluentAssertions;
using FluentValidation.TestHelper;
using PANDA.Api.Common;
using PANDA.Api.Dto;
using PANDA.Api.Validation;

namespace PANDA.Api.Tests.Validation;

public class PatientDtoValidatorTests
{
    private readonly PatientDtoValidator _validator = new();
    private const string ValidNhsNumber = "9434765919";

    [Fact]
    public void Should_Pass_Validation_With_Valid_Data()
    {
        var dto = new PatientDto
        {
            FirstName = "Alice",
            LastName = "Smith",
            DateOfBirth = new DateTime(1990, 1, 1),
            NHSNumber = ValidNhsNumber,
            Postcode = "AB12 3CD",
            Gender = Gender.Female
        };

        var result = _validator.TestValidate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_If_Required_Fields_Are_Missing()
    {
        var dto = new PatientDto();

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        result.ShouldHaveValidationErrorFor(x => x.NHSNumber);
        result.ShouldHaveValidationErrorFor(x => x.Postcode);
        result.ShouldHaveValidationErrorFor(x => x.Gender);
    }

    [Fact]
    public void Should_Fail_If_DateOfBirth_Is_In_The_Future()
    {
        var dto = new PatientDto
        {
            FirstName = "Bob",
            LastName = "Brown",
            DateOfBirth = DateTime.Today.AddDays(1),
            NHSNumber = ValidNhsNumber,
            Postcode = "CD34 5EF",
            Gender = Gender.Male
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Should_Fail_If_NHSNumber_Is_Too_Short()
    {
        var dto = new PatientDto
        {
            FirstName = "Clara",
            LastName = "Jones",
            DateOfBirth = new DateTime(1980, 1, 1),
            NHSNumber = "12345",
            Postcode = "GH56 7IJ",
            Gender = Gender.Other
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NHSNumber);
    }

    [Fact]
    public void Should_Fail_If_Postcode_Is_Invalid()
    {
        var dto = new PatientDto
        {
            FirstName = "Donna",
            LastName = "Noble",
            DateOfBirth = new DateTime(1970, 1, 1),
            NHSNumber = ValidNhsNumber,
            Postcode = "BADPOST", // invalid
            Gender = Gender.Female
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Postcode);
    }
}
