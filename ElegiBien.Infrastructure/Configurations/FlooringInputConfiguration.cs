using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class FlooringInputConfiguration : IEntityTypeConfiguration<FlooringInput>
{
    public void Configure(EntityTypeBuilder<FlooringInput> builder)
    {
        builder.ToTable("FlooringInputs");
        builder.HasKey(x => x.FlooringInputId);
        builder.HasIndex(x => x.AnalysisId).IsUnique();
        builder.Property(x => x.LengthMeters).HasPrecision(8, 2);
        builder.Property(x => x.WidthMeters).HasPrecision(8, 2);
        builder.Property(x => x.WastePercentage).HasPrecision(5, 2);

        builder.HasOne(x => x.Analysis)
            .WithOne(x => x.FlooringInput)
            .HasForeignKey<FlooringInput>(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
