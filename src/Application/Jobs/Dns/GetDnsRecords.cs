using System.Collections.Immutable;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MillerDemo.Application.Persistence;

namespace MillerDemo.Application.Jobs.Dns;

/// <summary>
/// Represents a GetDnsRecords query.
/// </summary>
public record GetDnsRecordsQuery : IRequest<Result<ImmutableList<GetDnsRecordsResultItem>>>;

/// <summary>
/// Represents a GetDnsRecords handler.
/// </summary>
/// <param name="dbContext">The database context.</param>
public sealed class GetDnsRecordsHandler(IApplicationDbContext dbContext) : IRequestHandler<GetDnsRecordsQuery, Result<ImmutableList<GetDnsRecordsResultItem>>>
{
    /// <summary>
    /// Handles the GetDnsRecords query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task that contains an immutable list of result items.</returns>
    public async Task<Result<ImmutableList<GetDnsRecordsResultItem>>> Handle(GetDnsRecordsQuery query, CancellationToken cancellationToken)
    {
        var dnsRecords = await dbContext.DnsRecords.OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        var items = dnsRecords.Select(x => new GetDnsRecordsResultItem(x.Name, x.IpAddress))
            .ToImmutableList();

        return items;
    }
}

/// <summary>
/// Represents a GetDnsRecords result item.
/// </summary>
/// <param name="Name">The DNS name.</param>
/// <param name="IpAddress">The IP address.</param>
public record GetDnsRecordsResultItem(string Name, string IpAddress);