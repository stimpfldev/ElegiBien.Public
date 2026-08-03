using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class ProductScoreConfiguration
    : IEntityTypeConfiguration<ProductScore>
{
    public void Configure(EntityTypeBuilder<ProductScore> builder)
    {
        builder.ToTable("ProductScores");

        builder.HasKey(x => x.ProductScoreId);

        builder.HasOne(x => x.ProductAlternative)
            .WithOne(x => x.ProductScore)
            .HasForeignKey<ProductScore>(x => x.ProductAlternativeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProductAlternativeId)
            .IsUnique();
    }
}