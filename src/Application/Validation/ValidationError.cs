using FluentResults;

namespace MillerDemo.Application.Validation;

/// <summary>
/// Represents a validation error.
/// </summary>
public sealed class ValidationError : Error
{
    /// <summary>
    /// Gets a dictionary containing the list of validation errors.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ValidationError"/> with the specified validation errors.
    /// </summary>
    /// <param name="errors">The validation errors.</param>
    public ValidationError(IDictionary<string, string[]> errors)
        : base("The request could not be processed due to validation errors.")
    {
        Errors = errors;
    }
}