using FluentAssertions;
using PANDA.Api.Helpers;

namespace PANDA.Api.Tests.Helpers;

public class ValidationHelperTests
{
    [Theory]
    [InlineData("4010232137", true)]
    [InlineData("9876543210", true)]
    [InlineData("abcdefghij", false)]
    [InlineData("12345", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidNHSNumber_ShouldValidateCorrectly(string nhsNumber, bool expected)
    {
        var result = ValidationHelper.IsValidNHSNumber(nhsNumber);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("SW1A 1AA", true)]
    [InlineData("sw1a 1aa", true)]
    [InlineData("EC1A 1BB", true)]
    [InlineData("INVALID", false)]
    [InlineData("12345", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidPostcode_ShouldValidateCorrectly(string postcode, bool expected)
    {
        var result = ValidationHelper.IsValidPostcode(postcode);
        result.Should().Be(expected);
    }
}