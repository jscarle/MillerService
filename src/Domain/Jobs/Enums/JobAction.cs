namespace MillerDemo.Domain.Jobs.Enums;

/// <summary>
/// Represents a job action.
/// </summary>
public enum JobAction
{
    /// <summary>
    /// Create a DNS record.
    /// </summary>
    CreateDnsRecord,

    /// <summary>
    /// Update a DNS record.
    /// </summary>
    UpdateDnsRecord,

    /// <summary>
    /// Delete a DNS record.
    /// </summary>
    DeleteDnsRecord
}