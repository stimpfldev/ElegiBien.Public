using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class ScoreFactorConfiguration
    : IEntityTypeConfiguration<ScoreFactor>
{
    public void Configure(EntityTypeBuilder<ScoreFactor> builder)
    {
        builder.ToTable("ScoreFactors");

        builder.HasKey(x => x.ScoreFactorId);

        builder.Property(x => x.Score)
            .HasPrecision(6, 2);

        builder.Property(x => x.MaximumScore)
            .HasPrecision(6, 2);

        builder.Property(x => x.Explanation)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(x => x.ProductScore)
            .WithMany(x => x.Factors)
            .HasForeignKey(x => x.ProductScoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}