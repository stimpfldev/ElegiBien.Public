using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class ComparisonFactorConfiguration : IEntityTypeConfiguration<ComparisonFactor>
{
    public void Configure(EntityTypeBuilder<ComparisonFactor> builder)
    {
        builder.ToTable("ComparisonFactors");
        builder.HasKey(x => x.ComparisonFactorId);
        builder.Property(x => x.FactorCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Label).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Score).HasPrecision(8, 2);
        builder.Property(x => x.MaximumScore).HasPrecision(8, 2);
        builder.Property(x => x.Weight).HasPrecision(8, 4);
        builder.Property(x => x.Explanation).HasMaxLength(500).IsRequired();
        builder.HasIndex(x => new { x.ComparisonScoreId, x.FactorCode }).IsUnique();
        builder.HasOne(x => x.ComparisonScore)
            .WithMany(x => x.Factors)
            .HasForeignKey(x => x.ComparisonScoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
