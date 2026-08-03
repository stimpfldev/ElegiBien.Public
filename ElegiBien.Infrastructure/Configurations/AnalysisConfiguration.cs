using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class AnalysisConfiguration : IEntityTypeConfiguration<Analysis>
{
    public void Configure(EntityTypeBuilder<Analysis> builder)
    {
        builder.ToTable("Analyses");

        builder.HasKey(x => x.AnalysisId);

        builder.Property(x => x.Mode)
            .IsRequired();

        builder.Property(x => x.ConfidenceLevel)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MethodologyVersion)
            .WithMany()
            .HasForeignKey(x => x.MethodologyVersionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.CreatedAtUtc);

        builder.HasIndex(x => new
        {
            x.CategoryId,
            x.CreatedAtUtc
        });
    }
}