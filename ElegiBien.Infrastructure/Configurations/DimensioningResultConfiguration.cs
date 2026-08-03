using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class DimensioningResultConfiguration
    : IEntityTypeConfiguration<DimensioningResult>
{
    public void Configure(EntityTypeBuilder<DimensioningResult> builder)
    {
        builder.ToTable("DimensioningResults");

        builder.HasKey(x => x.DimensioningResultId);

        builder.Property(x => x.VolumeCubicMeters)
            .HasPrecision(10, 2);

        builder.Property(x => x.BaseFrigories)
            .HasPrecision(12, 2);

        builder.Property(x => x.AdjustmentFrigories)
            .HasPrecision(12, 2);

        builder.Property(x => x.EstimatedFrigories)
            .HasPrecision(12, 2);

        builder.Property(x => x.RecommendedMinimumFrigories)
            .HasPrecision(12, 2);

        builder.Property(x => x.RecommendedMaximumFrigories)
            .HasPrecision(12, 2);

        builder.Property(x => x.IdealFrigories)
            .HasPrecision(12, 2);

        builder.HasOne(x => x.Analysis)
            .WithOne(x => x.DimensioningResult)
            .HasForeignKey<DimensioningResult>(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.AnalysisId)
            .IsUnique();
    }
}