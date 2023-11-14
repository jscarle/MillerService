using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MillerDemo.Domain.Jobs.Entities;

namespace MillerDemo.Infrastructure.Persistence.Jobs;

/// <summary>
/// Represents the configuration for persisting a <see cref="Job"/>.
/// </summary>
public sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Metadata)
            .IsUnicode(false)
            .HasMaxLength(255);
        builder.Property(x => x.FailureReason)
            .IsUnicode(false)
            .HasMaxLength(255);
    }
}