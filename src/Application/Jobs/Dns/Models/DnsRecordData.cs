using System.Runtime.Serialization;
using System.Text.Json;

namespace MillerDemo.Application.Jobs.Dns.Models;

/// <summary>
/// Represents DNS record data.
/// </summary>
/// <param name="Name">The DNS name.</param>
/// <param name="IpAddress">The IP address.</param>
public record DnsRecordData(string Name, string IpAddress)
{
    /// <summary>
    /// Converts the DNS record data to its string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// Converts a string representation to DNS record data.
    /// </summary>
    /// <param name="s">The string representation.</param>
    /// <returns>The DNS record data.</returns>
    public static DnsRecordData FromString(string s)
    {
        return JsonSerializer.Deserialize<DnsRecordData>(s) ?? throw new SerializationException($"Could not deserialize the string into an instance of {nameof(DnsRecordData)}.");
    }
}