using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MillerDemo.Application.Validation;

namespace MillerDemo.Api.Validation;

/// <summary>
/// Contains extension methods for fluent results.
/// </summary>
public static class FluentResultExtensions
{
    /// <summary>
    /// Gets the problem details for a failed result.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <returns>The problem details HTTP result.</returns>
    public static ProblemHttpResult ProblemDetails(this ResultBase result)
    {
        // 400 Bad Request
        if (result.HasError<ValidationError>())
        {
            var validationError = (ValidationError)result.Errors.First(e => e is ValidationError);
            var validationProblemDetails = new ValidationProblemDetails(validationError.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Bad Request",
                Detail = validationError.Message
            };
            return TypedResults.Problem(validationProblemDetails);
        }

        // 500 Internal Server Error
        var unhandledError = result.Errors.First();
        var unhandledErrorDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Internal Server Error",
            Detail = unhandledError.Message
        };
        return TypedResults.Problem(unhandledErrorDetails);
    }
}