using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class HeatingCalculationResultConfiguration :
    IEntityTypeConfiguration<HeatingCalculationResult>
{
    public void Configure(EntityTypeBuilder<HeatingCalculationResult> builder)
    {
        builder.ToTable("HeatingCalculationResults");
        builder.HasKey(x => x.HeatingCalculationResultId);
        builder.HasIndex(x => x.AnalysisId).IsUnique();
        builder.Property(x => x.SurfaceSquareMeters).HasPrecision(12, 2);
        builder.Property(x => x.VolumeCubicMeters).HasPrecision(12, 2);
        builder.Property(x => x.BasePowerWatts).HasPrecision(12, 2);
        builder.Property(x => x.AdjustmentPowerWatts).HasPrecision(12, 2);
        builder.Property(x => x.EstimatedPowerWatts).HasPrecision(12, 2);
        builder.Property(x => x.RecommendedMinimumWatts).HasPrecision(12, 2);
        builder.Property(x => x.RecommendedMaximumWatts).HasPrecision(12, 2);
        builder.Property(x => x.IdealPowerWatts).HasPrecision(12, 2);
        builder.Property(x => x.IdealPowerKcalPerHour).HasPrecision(12, 2);

        builder.HasOne(x => x.Analysis)
            .WithOne(x => x.HeatingCalculationResult)
            .HasForeignKey<HeatingCalculationResult>(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
