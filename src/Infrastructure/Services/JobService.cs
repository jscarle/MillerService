using FluentResults;
using Microsoft.EntityFrameworkCore;
using MillerDemo.Application.Jobs.Dns.Models;
using MillerDemo.Application.Persistence;
using MillerDemo.Domain.Jobs.Entities;
using MillerDemo.Domain.Jobs.Enums;

namespace MillerDemo.Infrastructure.Services;

/// <summary>
/// Defines a service that processes queued jobs.
/// </summary>
/// <param name="serviceProvider">The service provider.</param>
/// <param name="dnsService">The DNS service.</param>
/// <param name="logger">The logger.</param>
public class JobService(IServiceProvider serviceProvider, IDnsService dnsService, ILogger<JobService> logger) : IHostedService, IDisposable
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private Timer? _timer;

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(ProcessJobs, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _timer?.Dispose();
    }

    private async void ProcessJobs(object? state)
    {
        try
        {
            await _lock.WaitAsync();

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            await ProcessPendingJobsAsync(dbContext);
            await RemovePastJobsAsync(dbContext);
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Critical, ex, "{Message}", $"{ex.GetType().Name}: {ex.Message}");
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task ProcessPendingJobsAsync(IApplicationDbContext dbContext)
    {
        var pendingJobs = await dbContext.Jobs
            .Where(x => x.State == JobState.Created)
            .OrderBy(x => x.Created)
            .ToListAsync();

        foreach (var job in pendingJobs)
        {
            job.State = JobState.InProgress;
            await dbContext.SaveChangesAsync();

            Result result;
            switch (job.Action)
            {
                case JobAction.CreateDnsRecord:
                {
                    var dnsRecordData = DnsRecordData.FromString(job.Metadata!);
                    result = await dnsService.CreateDnsRecordAsync(dnsRecordData.Name, dnsRecordData.IpAddress);
                    break;
                }
                case JobAction.UpdateDnsRecord:
                {
                    var dnsRecordData = DnsRecordData.FromString(job.Metadata!);
                    result = await dnsService.UpdateDnsRecordAsync(dnsRecordData.Name, dnsRecordData.IpAddress);
                    break;
                }
                case JobAction.DeleteDnsRecord:
                {
                    var dnsRecordName = job.Metadata!;
                    result = await dnsService.DeleteDnsRecordAsync(dnsRecordName);
                    break;
                }
                default:
                    throw new NotImplementedException();
            }

            await SetJobStateAsync(dbContext, job, result);
        }
    }

    private static Task SetJobStateAsync(IApplicationDbContext dbContext, Job job, IResultBase result)
    {
        if (result.IsSuccess)
        {
            job.State = JobState.Completed;
        }
        else
        {
            job.State = JobState.Failed;
            job.FailureReason = result.Errors.First().Message;
        }

        return dbContext.SaveChangesAsync();
    }

    private static async Task RemovePastJobsAsync(IApplicationDbContext dbContext)
    {
        var limit = DateTime.Now.Subtract(TimeSpan.FromMinutes(5));

        var completedJobs = await dbContext.Jobs
            .Where(x => x.State != JobState.Created && x.Created <= limit)
            .ToListAsync();

        dbContext.Jobs.RemoveRange(completedJobs);

        await dbContext.SaveChangesAsync();
    }
}