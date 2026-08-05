using ElegiBien.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public DbSet<PaintInput> PaintInputs => Set<PaintInput>();

    public DbSet<PaintCalculationResult> PaintCalculationResults =>
        Set<PaintCalculationResult>();

    public DbSet<FlooringInput> FlooringInputs => Set<FlooringInput>();

    public DbSet<FlooringCalculationResult> FlooringCalculationResults =>
        Set<FlooringCalculationResult>();

    public DbSet<HeatingInput> HeatingInputs =>
        Set<HeatingInput>();

    public DbSet<HeatingCalculationResult> HeatingCalculationResults =>
        Set<HeatingCalculationResult>();

    public DbSet<ComparisonAlternative> ComparisonAlternatives =>
        Set<ComparisonAlternative>();

    public DbSet<ComparisonScore> ComparisonScores =>
        Set<ComparisonScore>();

    public DbSet<ComparisonFactor> ComparisonFactors =>
        Set<ComparisonFactor>();

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
