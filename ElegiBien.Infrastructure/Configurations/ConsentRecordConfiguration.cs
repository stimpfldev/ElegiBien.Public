using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class ConsentRecordConfiguration
    : IEntityTypeConfiguration<ConsentRecord>
{
    public void Configure(EntityTypeBuilder<ConsentRecord> builder)
    {
        builder.ToTable("ConsentRecords");

        builder.HasKey(x => x.ConsentRecordId);

        builder.Property(x => x.ConsentType)
            .IsRequired();

        builder.Property(x => x.LegalVersion)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.RecordedAtUtc)
            .IsRequired();

        builder.HasOne(x => x.Analysis)
            .WithMany(x => x.ConsentRecords)
            .HasForeignKey(x => x.AnalysisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.AnalysisId,
            x.ConsentType
        });

        builder.HasIndex(x => x.RecordedAtUtc);
    }
}