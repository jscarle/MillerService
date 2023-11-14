namespace MillerDemo.Domain.DnsRecords.Entities;

public sealed class DnsRecord
{
    public required string Name { get; set; }
    public required string IpAddress { get; set; }
}