using MillerDemo.Domain.Jobs.Enums;

namespace MillerDemo.Domain.Jobs.Entities;

/// <summary>
/// Represents a job.
/// </summary>
public sealed class Job
{
    /// <summary>
    /// Gets the job identifier.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets the job action.
    /// </summary>
    public required JobAction Action { get; init; }

    /// <summary>
    /// Gets the job metadata.
    /// </summary>
    public string? Metadata { get; init; }

    /// <summary>
    /// Gets or sets the job state.
    /// </summary>
    public JobState State { get; set; } = JobState.Created;

    /// <summary>
    /// Gets or sets the reason for the failure when the job is in a failed state.
    /// </summary>
    public string? FailureReason { get; set; }

    /// <summary>
    /// Gets the date and time the job was created.
    /// </summary>
    public DateTime Created { get; init; } = DateTime.Now;
}