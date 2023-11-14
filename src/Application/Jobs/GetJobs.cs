using System.Collections.Immutable;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MillerDemo.Application.Jobs.Dns.Models;
using MillerDemo.Application.Persistence;
using MillerDemo.Domain.Jobs.Enums;

namespace MillerDemo.Application.Jobs;

/// <summary>
/// Represents a GetJobs query.
/// </summary>
public record GetJobsQuery : IRequest<Result<ImmutableList<GetJobsResultItem>>>;

/// <summary>
/// Represents a GetJobs handler.
/// </summary>
/// <param name="dbContext">The database context.</param>
public sealed class GetJobsHandler(IApplicationDbContext dbContext) : IRequestHandler<GetJobsQuery, Result<ImmutableList<GetJobsResultItem>>>
{
    /// <summary>
    /// Handles the GetJobs query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task that contains an immutable list of result items.</returns>
    /// <exception cref="NotImplementedException">Thrown when the job action or job state has not been implemented.</exception>
    public async Task<Result<ImmutableList<GetJobsResultItem>>> Handle(GetJobsQuery query, CancellationToken cancellationToken)
    {
        var jobs = await dbContext.Jobs.OrderBy(x => x.Created)
            .ToListAsync(cancellationToken);

        var items = jobs.Select(job =>
            {
                string description;
                switch (job.Action)
                {
                    case JobAction.CreateDnsRecord:
                    {
                        var dnsRecord = DnsRecordData.FromString(job.Metadata!);
                        description =
                            $"Create DNS record '{dnsRecord.Name}' with IP address '{dnsRecord.IpAddress}'";
                        break;
                    }
                    case JobAction.UpdateDnsRecord:
                    {
                        var dnsRecord = DnsRecordData.FromString(job.Metadata!);
                        description =
                            $"Update DNS record '{dnsRecord.Name}' with IP address '{dnsRecord.IpAddress}'";
                        break;
                    }
                    case JobAction.DeleteDnsRecord:
                    {
                        var dnsRecordName = job.Metadata!;
                        description =
                            $"Delete DNS record '{dnsRecordName}'";
                        break;
                    }
                    default:
                        throw new NotImplementedException();
                }

                var state = job.State switch
                {
                    JobState.Created => "Created",
                    JobState.InProgress => "In Progress",
                    JobState.Completed => "Completed",
                    JobState.Failed => $"Failed: {job.FailureReason}",
                    _ => throw new NotImplementedException()
                };

                return new GetJobsResultItem(job.Id, description, state);
            })
            .ToImmutableList();

        return items;
    }
}

/// <summary>
/// Represents a GetJobs result item.
/// </summary>
/// <param name="Id">The job identifier.</param>
/// <param name="Description">The job description.</param>
/// <param name="State">The job state.</param>
public record GetJobsResultItem(Guid Id, string Description, string State);