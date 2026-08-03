using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class ProductAlternativeConfiguration
    : IEntityTypeConfiguration<ProductAlternative>
{
    public void Configure(EntityTypeBuilder<ProductAlternative> builder)
    {
        builder.ToTable("ProductAlternatives");

        builder.HasKey(x => x.ProductAlternativeId);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Brand)
            .HasMaxLength(100);

        builder.Property(x => x.CapacityFrigories)
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.NominalConsumptionWatts)
            .HasPrecision(12, 2);

        builder.Property(x => x.ReferenceUrl)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Analysis)
            .WithMany(x => x.ProductAlternatives)
            .HasForeignKey(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}