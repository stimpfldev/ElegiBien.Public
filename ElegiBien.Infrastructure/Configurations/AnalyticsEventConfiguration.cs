using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class AnalyticsEventConfiguration
    : IEntityTypeConfiguration<AnalyticsEvent>
{
    public void Configure(EntityTypeBuilder<AnalyticsEvent> builder)
    {
        builder.ToTable("AnalyticsEvents");

        builder.HasKey(x => x.AnalyticsEventId);

        builder.Property(x => x.EventType)
            .IsRequired();

        builder.Property(x => x.OccurredAtUtc)
            .IsRequired();

        builder.HasOne(x => x.Analysis)
            .WithMany(x => x.AnalyticsEvents)
            .HasForeignKey(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.OccurredAtUtc);

        builder.HasIndex(x => new
        {
            x.EventType,
            x.OccurredAtUtc
        });

        builder.HasIndex(x => new
        {
            x.CategoryId,
            x.OccurredAtUtc
        });
    }
}