using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MillerDemo.Domain.DnsRecords.Entities;

namespace MillerDemo.Infrastructure.Persistence.DnsRecords;

/// <summary>
/// Represents the configuration for persisting a <see cref="DnsRecord"/>.
/// </summary>
public sealed class DnsRecordConfiguration : IEntityTypeConfiguration<DnsRecord>
{
    public void Configure(EntityTypeBuilder<DnsRecord> builder)
    {
        builder.HasKey(x => x.Name);

        builder.Property(x => x.Name)
            .IsUnicode(false)
            .HasMaxLength(64);
        builder.Property(x => x.IpAddress)
            .IsUnicode(false)
            .HasMaxLength(15);
    }
}