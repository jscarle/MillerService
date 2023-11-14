namespace MillerDemo.Domain.Jobs.Enums;

/// <summary>
/// Represents the job state.
/// </summary>
public enum JobState
{
    /// <summary>
    /// Created.
    /// </summary>
    Created,

    /// <summary>
    /// In progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Completed.
    /// </summary>
    Completed,

    /// <summary>
    /// Failed.
    /// </summary>
    Failed
}