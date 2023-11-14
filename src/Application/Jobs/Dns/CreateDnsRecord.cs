using System.Net;
using System.Text.RegularExpressions;
using FluentResults;
using FluentValidation;
using MediatR;
using MillerDemo.Application.Jobs.Dns.Models;
using MillerDemo.Application.Persistence;
using MillerDemo.Domain.Jobs.Entities;
using MillerDemo.Domain.Jobs.Enums;

namespace MillerDemo.Application.Jobs.Dns;

/// <summary>
/// Represents a CreateDnsRecord command.
/// </summary>
/// <param name="Name">The DNS name.</param>
/// <param name="IpAddress">The IP address.</param>
public record CreateDnsRecordCommand(string Name, string IpAddress) : IRequest<Result>;

/// <summary>
/// Represents a CreateDnsRecord validator.
/// </summary>
public sealed partial class CreateDnsRecordValidator : AbstractValidator<CreateDnsRecordCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CreateDnsRecordValidator"/>.
    /// </summary>
    public CreateDnsRecordValidator()
    {
        RuleFor(x => x.Name).Must((_, s, context) =>
        {
            if (s is null || !NameRegex().IsMatch(s))
                context.AddFailure($"'{context.PropertyPath}' must be a valid name.");
            return true;
        });
        RuleFor(x => x.IpAddress).Must((r, s, context) =>
        {
            if (s is null || !IPAddress.TryParse(s, out _))
                context.AddFailure($"'{context.PropertyPath}' must be a valid IP address.");
            return true;
        });
    }

    [GeneratedRegex(@"^(?!-)(?:[a-z0-9-]{1,63}|xn--[a-z0-9]{1,59})$")]
    private static partial Regex NameRegex();
}

/// <summary>
/// Represents a CreateDnsRecord handler.
/// </summary>
/// <param name="dbContext">The database context.</param>
public sealed class CreateDnsRecordHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateDnsRecordCommand, Result>
{
    /// <summary>
    /// Handles the CreateDnsRecord command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task that contains the result of the command.</returns>
    public async Task<Result> Handle(CreateDnsRecordCommand command, CancellationToken cancellationToken)
    {
        var dnsRecord = new DnsRecordData(command.Name, command.IpAddress);

        var job = new Job
        {
            Action = JobAction.CreateDnsRecord,
            Metadata = dnsRecord.ToString()
        };
        dbContext.Jobs.Add(job);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}