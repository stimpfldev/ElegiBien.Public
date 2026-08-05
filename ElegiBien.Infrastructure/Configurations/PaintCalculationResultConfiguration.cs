using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class PaintCalculationResultConfiguration : IEntityTypeConfiguration<PaintCalculationResult>
{
    public void Configure(EntityTypeBuilder<PaintCalculationResult> builder)
    {
        builder.ToTable("PaintCalculationResults");
        builder.HasKey(x => x.PaintCalculationResultId);
        builder.HasIndex(x => x.AnalysisId).IsUnique();
        builder.Property(x => x.WallAreaSquareMeters).HasPrecision(12, 2);
        builder.Property(x => x.CeilingAreaSquareMeters).HasPrecision(12, 2);
        builder.Property(x => x.DeductedAreaSquareMeters).HasPrecision(12, 2);
        builder.Property(x => x.NetAreaSquareMeters).HasPrecision(12, 2);
        builder.Property(x => x.AdjustedAreaSquareMeters).HasPrecision(12, 2);
        builder.Property(x => x.ReferenceCoverageSquareMetersPerLiter).HasPrecision(8, 2);
        builder.Property(x => x.ReferenceLiters).HasPrecision(10, 2);
        builder.HasOne(x => x.Analysis).WithOne(x => x.PaintCalculationResult).HasForeignKey<PaintCalculationResult>(x => x.AnalysisId).OnDelete(DeleteBehavior.Cascade);
    }
}
