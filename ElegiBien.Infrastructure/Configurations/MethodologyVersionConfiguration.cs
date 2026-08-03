using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElegiBien.Infrastructure.Configurations;

public class MethodologyVersionConfiguration
    : IEntityTypeConfiguration<MethodologyVersion>
{
    public void Configure(EntityTypeBuilder<MethodologyVersion> builder)
    {
        builder.ToTable("MethodologyVersions");

        builder.HasKey(x => x.MethodologyVersionId);

        builder.Property(x => x.Version)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.CategoryId,
            x.Version
        }).IsUnique();
    }
}