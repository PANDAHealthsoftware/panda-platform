using Microsoft.AspNetCore.Mvc;

namespace PANDAView.Helpers
{
    public class ApiValidationException : Exception
    {
        public readonly Dictionary<string, string[]> Errors;

        public ApiValidationException(ValidationProblemDetails details)
            : base("One or more validation errors occurred.")
        {
            Errors = details.Errors.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}