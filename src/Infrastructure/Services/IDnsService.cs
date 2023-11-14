using FluentResults;

namespace MillerDemo.Infrastructure.Services;

/// <summary>
/// Defines a DNS service.
/// </summary>
public interface IDnsService
{
    /// <summary>
    /// Creates a DNS record.
    /// </summary>
    /// <param name="name">The DNS name.</param>
    /// <param name="ipAddress">The IP address.</param>
    /// <returns>An awaitable task with the result of the operation.</returns>
    Task<Result> CreateDnsRecordAsync(string name, string ipAddress);

    /// <summary>
    /// Updates a DNS record.
    /// </summary>
    /// <param name="name">The DNS name.</param>
    /// <param name="ipAddress">The IP address.</param>
    /// <returns>An awaitable task with the result of the operation.</returns>
    Task<Result> UpdateDnsRecordAsync(string name, string ipAddress);

    /// <summary>
    /// Deletes a DNS record.
    /// </summary>
    /// <param name="name">The DNS name.</param>
    /// <returns>An awaitable task with the result of the operation.</returns>
    Task<Result> DeleteDnsRecordAsync(string name);
}