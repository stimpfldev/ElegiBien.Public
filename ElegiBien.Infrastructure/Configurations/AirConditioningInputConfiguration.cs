using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class AirConditioningInputConfiguration
    : IEntityTypeConfiguration<AirConditioningInput>
{
    public void Configure(EntityTypeBuilder<AirConditioningInput> builder)
    {
        builder.ToTable("AirConditioningInputs");

        builder.HasKey(x => x.AirConditioningInputId);

        builder.Property(x => x.LengthMeters)
            .HasPrecision(8, 2)
            .IsRequired();

        builder.Property(x => x.WidthMeters)
            .HasPrecision(8, 2)
            .IsRequired();

        builder.Property(x => x.HeightMeters)
            .HasPrecision(8, 2)
            .IsRequired();

        builder.HasOne(x => x.Analysis)
            .WithOne(x => x.AirConditioningInput)
            .HasForeignKey<AirConditioningInput>(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.AnalysisId)
            .IsUnique();
    }
}