using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ElegiBien.Infrastructure.Data;

public class ElegiBienDbContext : DbContext
{
    public ElegiBienDbContext(
        DbContextOptions<ElegiBienDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<MethodologyVersion> MethodologyVersions =>
        Set<MethodologyVersion>();

    public DbSet<Analysis> Analyses => Set<Analysis>();

    public DbSet<AirConditioningInput> AirConditioningInputs =>
        Set<AirConditioningInput>();

    public DbSet<DimensioningResult> DimensioningResults =>
        Set<DimensioningResult>();

    public DbSet<ProductAlternative> ProductAlternatives =>
        Set<ProductAlternative>();

    public DbSet<ProductScore> ProductScores =>
        Set<ProductScore>();

    public DbSet<ScoreFactor> ScoreFactors =>
        Set<ScoreFactor>();

    public DbSet<SharedResult> SharedResults =>
        Set<SharedResult>();

    public DbSet<ConsentRecord> ConsentRecords =>
        Set<ConsentRecord>();

    public DbSet<AnalyticsEvent> AnalyticsEvents =>
        Set<AnalyticsEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ElegiBienDbContext).Assembly);
    }
}