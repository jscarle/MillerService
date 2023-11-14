using System.Text.RegularExpressions;
using FluentResults;
using FluentValidation;
using MediatR;
using MillerDemo.Application.Persistence;
using MillerDemo.Domain.Jobs.Entities;
using MillerDemo.Domain.Jobs.Enums;

namespace MillerDemo.Application.Jobs.Dns;

/// <summary>
/// Represents a DeleteDnsRecord command.
/// </summary>
/// <param name="Name">The DNS name.</param>
public record DeleteDnsRecordCommand(string Name) : IRequest<Result>;

/// <summary>
/// Represents a DeleteDnsRecord validator.
/// </summary>
public sealed partial class DeleteDnsRecordValidator : AbstractValidator<DeleteDnsRecordCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="UpdateDnsRecordValidator"/>.
    /// </summary>
    public DeleteDnsRecordValidator()
    {
        RuleFor(x => x.Name).Must((_, s, context) =>
        {
            if (s is null || !NameRegex().IsMatch(s))
                context.AddFailure($"'{context.PropertyPath}' must be a valid name.");
            return true;
        });
    }

    [GeneratedRegex(@"^(?!-)(?:[a-z0-9-]{1,63}|xn--[a-z0-9]{1,59})$")]
    private static partial Regex NameRegex();
}

/// <summary>
/// Represents a DeleteDnsRecord handler.
/// </summary>
/// <param name="dbContext">The database context.</param>
public sealed class DeleteDnsRecordHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteDnsRecordCommand, Result>
{
    /// <summary>
    /// Handles the DeleteDnsRecord command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task that contains the result of the command.</returns>
    public async Task<Result> Handle(DeleteDnsRecordCommand command, CancellationToken cancellationToken)
    {
        var job = new Job
        {
            Action = JobAction.DeleteDnsRecord,
            Metadata = command.Name
        };
        dbContext.Jobs.Add(job);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}