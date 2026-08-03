using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class SharedResultConfiguration
    : IEntityTypeConfiguration<SharedResult>
{
    public void Configure(EntityTypeBuilder<SharedResult> builder)
    {
        builder.ToTable("SharedResults");

        builder.HasKey(x => x.SharedResultId);

        builder.Property(x => x.PublicToken)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.ExpiresAtUtc)
            .IsRequired();

        builder.HasOne(x => x.Analysis)
            .WithOne(x => x.SharedResult)
            .HasForeignKey<SharedResult>(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.AnalysisId)
            .IsUnique();

        builder.HasIndex(x => x.PublicToken)
            .IsUnique();

        builder.HasIndex(x => new
        {
            x.IsActive,
            x.ExpiresAtUtc
        });
    }
}