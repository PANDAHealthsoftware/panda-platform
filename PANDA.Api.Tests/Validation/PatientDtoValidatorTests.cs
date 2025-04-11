using FluentAssertions;
using FluentValidation.TestHelper;
using PANDA.Api.Validation;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs;

namespace PANDA.Api.Tests.Validation;

public class PatientDtoValidatorTests
{
    private readonly PatientDtoValidator _validator = new();
    private readonly CreatePatientDtoValidator _createValidator = new();

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var model = new PatientDto { FirstName = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        var model = new PatientDto { LastName = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Should_Have_Error_When_DateOfBirth_Is_Invalid()
    {
        var model = new PatientDto { DateOfBirth = DateTime.Today.AddDays(1) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Should_Have_Error_When_NHSNumber_Is_Invalid()
    {
        var model = new PatientDto { NHSNumber = "123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NHSNumber);
    }

    [Fact]
    public void Should_Have_Error_When_Postcode_Is_Invalid()
    {
        var model = new PatientDto { Postcode = "XYZ123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Postcode);
    }

    [Fact]
    public void Should_Have_Error_When_Gender_Is_Null()
    {
        var model = new PatientDto { Gender = null };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Gender);
    }

    [Fact]
    public void Should_Have_No_Errors_For_Valid_CreatePatientDto()
    {
        var model = new CreatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            NHSNumber = "9434765919",
            Postcode = "LS12 3AB",
            Gender = Gender.Male
        };

        var result = _createValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
