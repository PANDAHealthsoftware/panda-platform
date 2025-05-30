using Microsoft.AspNetCore.Mvc;

namespace PANDAView.Helpers
{
    /// <summary>
    /// Represents an exception that is thrown when API validation errors occur.
    /// </summary>
    /// <remarks>
    /// This exception wraps the validation errors returned from an API, allowing them to be easily accessed
    /// and handled. The errors are stored in a dictionary where the key is the field name and the value is an
    /// array of error messages associated with that field.
    /// </remarks>
    public class ApiValidationException : Exception
    {
        /// <summary>
        /// Represents a dictionary of errors where the key is a string and the value is an array of strings.
        /// Typically used to store validation errors returned by the server.
        /// </summary>
        public readonly Dictionary<string, string[]> Errors;

        /// Represents an exception that occurs when there are validation errors in an API response.
        /// This exception is typically used to handle and encapsulate validation errors, such as those
        /// returned in a `ValidationProblemDetails` object.
        public ApiValidationException(ValidationProblemDetails details)
            : base("One or more validation errors occurred.")
        {
            Errors = details.Errors.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}