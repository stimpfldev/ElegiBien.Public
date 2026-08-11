using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class PaintInputConfiguration : IEntityTypeConfiguration<PaintInput>
{
    public void Configure(EntityTypeBuilder<PaintInput> builder)
    {
        builder.ToTable("PaintInputs");
        builder.HasKey(x => x.PaintInputId);
        builder.HasIndex(x => x.AnalysisId).IsUnique();
        builder.Property(x => x.LengthMeters).HasPrecision(8, 2);
        builder.Property(x => x.WidthMeters).HasPrecision(8, 2);
        builder.Property(x => x.HeightMeters).HasPrecision(8, 2);
        builder.Property(x => x.WastePercentage).HasPrecision(5, 2);
        builder.HasOne(x => x.Analysis).WithOne(x => x.PaintInput).HasForeignKey<PaintInput>(x => x.AnalysisId).OnDelete(DeleteBehavior.Cascade);
    }
}
