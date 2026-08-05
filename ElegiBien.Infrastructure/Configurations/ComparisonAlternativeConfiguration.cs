using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class ComparisonAlternativeConfiguration : IEntityTypeConfiguration<ComparisonAlternative>
{
    public void Configure(EntityTypeBuilder<ComparisonAlternative> builder)
    {
        builder.ToTable("ComparisonAlternatives");
        builder.HasKey(x => x.ComparisonAlternativeId);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.TotalCost).HasPrecision(18, 2);
        builder.Property(x => x.DetailsJson).HasColumnType("nvarchar(max)").IsRequired();
        builder.HasIndex(x => new { x.AnalysisId, x.Position }).IsUnique();
        builder.HasIndex(x => x.CategoryCode);
        builder.HasOne(x => x.Analysis)
            .WithMany(x => x.ComparisonAlternatives)
            .HasForeignKey(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
