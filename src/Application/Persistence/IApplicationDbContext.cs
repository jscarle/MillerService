using Microsoft.EntityFrameworkCore;
using MillerDemo.Domain.DnsRecords.Entities;
using MillerDemo.Domain.Jobs.Entities;

namespace MillerDemo.Application.Persistence;

/// <summary>
/// Defines the database context for the application.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets DNS records.
    /// </summary>
    DbSet<DnsRecord> DnsRecords { get; }

    /// <summary>
    /// Gets jobs.
    /// </summary>
    DbSet<Job> Jobs { get; }

    /// <summary>
    /// Saves changes to the database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Migrates the database.
    /// </summary>
    /// <returns>An awaitable task.</returns>
    Task MigrateAsync(CancellationToken cancellationToken = default);
}