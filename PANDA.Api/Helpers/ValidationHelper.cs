using System.Text.RegularExpressions;

namespace PANDA.Api.Helpers;

public static class ValidationHelper
{
    private static readonly Regex PostcodeRegex = new Regex(@"^[A-Z]{1,2}\d[A-Z\d]? \d[A-Z]{2}$", RegexOptions.IgnoreCase);

    public static bool IsValidNHSNumber(string nhsNumber)
    {
        if (string.IsNullOrEmpty(nhsNumber) || nhsNumber.Length != 10 || !long.TryParse(nhsNumber, out _))
        {
            return false;
        }

        // Basic checksum validation: If you need the actual NHS checksum algorithm, it can be implemented here.
        // For now, assuming simple validation (you can extend this if needed).
        var checksum = nhsNumber.Select((c, i) => (c - '0') * (i + 1)).Sum() % 11;
        return checksum == 0;  // Assuming the NHS checksum rule
    }
    
    public static bool IsValidPostcode(string postcode)
    {
        return !string.IsNullOrWhiteSpace(postcode) && PostcodeRegex.IsMatch(postcode);
    }
}