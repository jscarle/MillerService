using FluentResults;
using MillerDemo.Application.Persistence;
using MillerDemo.Domain.DnsRecords.Entities;

namespace MillerDemo.Infrastructure.Services;

/// <summary>
/// Defines a fake DNS service to simulate interaction with a DNS provider.
/// </summary>
/// <param name="serviceProvider">The service provider.</param>
public sealed class FakeDnsService(IServiceProvider serviceProvider) : IDnsService
{
    /// <inheritdoc/>
    public async Task<Result> CreateDnsRecordAsync(string name, string ipAddress)
    {
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        // Simulate a processing delay.
        var delay = Random.Shared.Next(100, 5000);
        await Task.Delay(delay);

        var existingDnsRecord = await dbContext.DnsRecords.FindAsync(name);
        if (existingDnsRecord is not null)
            return Result.Fail("DNS record already exists.");

        var dnsRecord = new DnsRecord
        {
            Name = name,
            IpAddress = ipAddress
        };
        dbContext.DnsRecords.Add(dnsRecord);

        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }

    /// <inheritdoc/>
    public async Task<Result> UpdateDnsRecordAsync(string name, string ipAddress)
    {
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        // Simulate a processing delay.
        var delay = Random.Shared.Next(100, 5000);
        await Task.Delay(delay);

        var existingDnsRecord = await dbContext.DnsRecords.FindAsync(name);
        if (existingDnsRecord is null)
            return Result.Fail("DNS record does not exist.");

        existingDnsRecord.IpAddress = ipAddress;

        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteDnsRecordAsync(string name)
    {
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        // Simulate a processing delay.
        var delay = Random.Shared.Next(100, 5000);
        await Task.Delay(delay);

        var existingDnsRecord = await dbContext.DnsRecords.FindAsync(name);
        if (existingDnsRecord is null)
            return Result.Fail("DNS record does not exist.");

        dbContext.DnsRecords.Remove(existingDnsRecord);

        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}