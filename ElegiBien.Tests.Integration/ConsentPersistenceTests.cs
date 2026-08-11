using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using ElegiBien.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Tests.Integration;

public sealed class ConsentPersistenceTests
{
    [Fact]
    public async Task RecordConsent_PersistsBothConsentTypes()
    {
        await using var db = CreateDbContext();
        var analysis = CreateAnalysis();
        db.Analyses.Add(analysis);
        await db.SaveChangesAsync();

        var service = new AnonymousAnalyticsService(db);

        await service.RecordConsentAsync(
            analysis.AnalysisId,
            ConsentType.AnonymousAnalytics,
            true,
            "1.3.1");

        await service.RecordConsentAsync(
            analysis.AnalysisId,
            ConsentType.RadarData,
            false,
            "1.3.1");

        var records = await db.ConsentRecords
            .AsNoTracking()
            .Where(x => x.AnalysisId == analysis.AnalysisId)
            .OrderBy(x => x.ConsentType)
            .ToListAsync();

        Assert.Equal(2, records.Count);
        Assert.Contains(records, x =>
            x.ConsentType == ConsentType.AnonymousAnalytics &&
            x.IsGranted &&
            x.LegalVersion == "1.3.1");
        Assert.Contains(records, x =>
            x.ConsentType == ConsentType.RadarData &&
            !x.IsGranted &&
            x.LegalVersion == "1.3.1");
    }

    [Fact]
    public async Task RecordConsent_UpdatesExistingRecordWithoutDuplicatingIt()
    {
        await using var db = CreateDbContext();
        var analysis = CreateAnalysis();
        db.Analyses.Add(analysis);
        await db.SaveChangesAsync();

        var service = new AnonymousAnalyticsService(db);

        await service.RecordConsentAsync(
            analysis.AnalysisId,
            ConsentType.AnonymousAnalytics,
            false,
            "1.3.0");

        await service.RecordConsentAsync(
            analysis.AnalysisId,
            ConsentType.AnonymousAnalytics,
            true,
            "1.3.1");

        var records = await db.ConsentRecords
            .AsNoTracking()
            .Where(x =>
                x.AnalysisId == analysis.AnalysisId &&
                x.ConsentType == ConsentType.AnonymousAnalytics)
            .ToListAsync();

        var record = Assert.Single(records);
        Assert.True(record.IsGranted);
        Assert.Equal("1.3.1", record.LegalVersion);
    }

    [Fact]
    public async Task AnalyticsEvent_IsPersistedOnlyWhenConsentWasGranted()
    {
        await using var db = CreateDbContext();
        var deniedAnalysis = CreateAnalysis();
        var grantedAnalysis = CreateAnalysis();
        db.Analyses.AddRange(deniedAnalysis, grantedAnalysis);
        await db.SaveChangesAsync();

        var service = new AnonymousAnalyticsService(db);

        await service.RecordConsentAsync(
            deniedAnalysis.AnalysisId,
            ConsentType.AnonymousAnalytics,
            false,
            "1.3.1");
        await service.RecordEventAsync(
            deniedAnalysis.AnalysisId,
            AnalyticsEventType.DimensioningCompleted,
            AnalysisMode.Quick);

        await service.RecordConsentAsync(
            grantedAnalysis.AnalysisId,
            ConsentType.AnonymousAnalytics,
            true,
            "1.3.1");
        await service.RecordEventAsync(
            grantedAnalysis.AnalysisId,
            AnalyticsEventType.DimensioningCompleted,
            AnalysisMode.Quick);

        Assert.False(await db.AnalyticsEvents.AnyAsync(x =>
            x.AnalysisId == deniedAnalysis.AnalysisId));
        Assert.True(await db.AnalyticsEvents.AnyAsync(x =>
            x.AnalysisId == grantedAnalysis.AnalysisId));
    }

    private static ElegiBienDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ElegiBienDbContext>()
            .UseInMemoryDatabase($"ConsentTests_{Guid.NewGuid():N}")
            .Options;

        return new ElegiBienDbContext(options);
    }

    private static Analysis CreateAnalysis()
    {
        return new Analysis
        {
            AnalysisId = Guid.NewGuid(),
            CategoryId = 1,
            MethodologyVersionId = 1,
            Mode = AnalysisMode.Quick,
            ConfidenceLevel = ConfidenceLevel.Medium,
            CreatedAtUtc = DateTime.UtcNow,
            CompletedAtUtc = DateTime.UtcNow,
            IsCompleted = true
        };
    }
}
