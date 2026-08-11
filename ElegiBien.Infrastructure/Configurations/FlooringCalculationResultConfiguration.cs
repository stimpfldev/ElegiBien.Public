using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class FlooringCalculationResultConfiguration :
    IEntityTypeConfiguration<FlooringCalculationResult>
{
    public void Configure(EntityTypeBuilder<FlooringCalculationResult> builder)
    {
        builder.ToTable("FlooringCalculationResults");
        builder.HasKey(x => x.FlooringCalculationResultId);
        builder.HasIndex(x => x.AnalysisId).IsUnique();
        builder.Property(x => x.TotalAreaSquareMeters).HasPrecision(12, 2);
        builder.Property(x => x.WastePercentage).HasPrecision(5, 2);
        builder.Property(x => x.WasteAreaSquareMeters).HasPrecision(12, 2);
        builder.Property(x => x.RequiredAreaSquareMeters).HasPrecision(12, 2);

        builder.HasOne(x => x.Analysis)
            .WithOne(x => x.FlooringCalculationResult)
            .HasForeignKey<FlooringCalculationResult>(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
