using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class ComparisonScoreConfiguration : IEntityTypeConfiguration<ComparisonScore>
{
    public void Configure(EntityTypeBuilder<ComparisonScore> builder)
    {
        builder.ToTable("ComparisonScores");
        builder.HasKey(x => x.ComparisonScoreId);
        builder.Property(x => x.TotalScore).HasPrecision(8, 2);
        builder.Property(x => x.AppliedMaximumScore).HasPrecision(8, 2);
        builder.Property(x => x.StatusCode).HasMaxLength(100);
        builder.Property(x => x.DetailsJson).HasColumnType("nvarchar(max)").IsRequired();
        builder.HasIndex(x => x.ComparisonAlternativeId).IsUnique();
        builder.HasOne(x => x.Alternative)
            .WithOne(x => x.Score)
            .HasForeignKey<ComparisonScore>(x => x.ComparisonAlternativeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
