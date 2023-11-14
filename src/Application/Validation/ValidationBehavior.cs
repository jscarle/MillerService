using FluentResults;
using FluentValidation;
using MediatR;

namespace MillerDemo.Application.Validation;

/// <summary>
/// Represents a validation behavior.
/// </summary>
/// <param name="validators">The validators.</param>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResult">The result type.</typeparam>
public sealed class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResult>
    where TRequest : notnull
    where TResult : IResultBase, new()
{
    /// <summary>
    /// Handles the request and processes validation.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="next">The next request handler.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task containing the result.</returns>
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var failures = await ValidateAsync(request, cancellationToken);
        if (failures.Keys.Count == 0)
            return await next();

        var result = new TResult();
        var validationError = new ValidationError(failures);
        result.Reasons.Add(validationError);
        return result;
    }

    private async Task<Dictionary<string, string[]>> ValidateAsync(TRequest operation,
        CancellationToken cancellationToken)
    {
        var validations = from validator in validators
            let context = new ValidationContext<TRequest>(operation)
            select validator.ValidateAsync(context, cancellationToken);

        var validationResults = await Task.WhenAll(validations);
        if (validationResults.Length == 0)
            return new Dictionary<string, string[]>();

        var failures = validationResults.Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

        return failures;
    }
}