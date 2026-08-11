using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class HeatingInputConfiguration : IEntityTypeConfiguration<HeatingInput>
{
    public void Configure(EntityTypeBuilder<HeatingInput> builder)
    {
        builder.ToTable("HeatingInputs");
        builder.HasKey(x => x.HeatingInputId);
        builder.HasIndex(x => x.AnalysisId).IsUnique();
        builder.Property(x => x.LengthMeters).HasPrecision(8, 2);
        builder.Property(x => x.WidthMeters).HasPrecision(8, 2);
        builder.Property(x => x.HeightMeters).HasPrecision(8, 2);

        builder.HasOne(x => x.Analysis)
            .WithOne(x => x.HeatingInput)
            .HasForeignKey<HeatingInput>(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
