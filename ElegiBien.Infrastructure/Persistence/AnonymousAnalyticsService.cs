using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class AnonymousAnalyticsService
    : IAnonymousAnalyticsService
{
    private readonly ElegiBienDbContext _dbContext;

    public AnonymousAnalyticsService(
        ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task RecordConsentAsync(
        Guid analysisId,
        ConsentType consentType,
        bool isGranted,
        string legalVersion,
        CancellationToken cancellationToken = default)
    {
        var analysisExists = await _dbContext.Analyses
            .AnyAsync(
                x => x.AnalysisId == analysisId,
                cancellationToken);

        if (!analysisExists)
        {
            throw new InvalidOperationException(
                "No se encontró el análisis asociado.");
        }

        var existing = await _dbContext.ConsentRecords
            .SingleOrDefaultAsync(
                x =>
                    x.AnalysisId == analysisId &&
                    x.ConsentType == consentType,
                cancellationToken);

        if (existing is null)
        {
            _dbContext.ConsentRecords.Add(
                new ConsentRecord
                {
                    AnalysisId = analysisId,
                    ConsentType = consentType,
                    IsGranted = isGranted,
                    LegalVersion = legalVersion,
                    RecordedAtUtc = DateTime.UtcNow
                });
        }
        else
        {
            existing.IsGranted = isGranted;
            existing.LegalVersion = legalVersion;
            existing.RecordedAtUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }

    public async Task RecordEventAsync(
        Guid analysisId,
        AnalyticsEventType eventType,
        AnalysisMode? mode,
        CancellationToken cancellationToken = default)
    {
        var analysis = await _dbContext.Analyses
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.AnalysisId == analysisId,
                cancellationToken);

        if (analysis is null)
        {
            throw new InvalidOperationException(
                "No se encontró el análisis asociado.");
        }

        var analyticsConsent = await _dbContext.ConsentRecords
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.AnalysisId == analysisId &&
                    x.ConsentType ==
                        ConsentType.AnonymousAnalytics &&
                    x.IsGranted,
                cancellationToken);

        if (!analyticsConsent)
        {
            return;
        }

        _dbContext.AnalyticsEvents.Add(
            new AnalyticsEvent
            {
                AnalysisId = analysisId,
                CategoryId = analysis.CategoryId,
                EventType = eventType,
                Mode = mode,
                OccurredAtUtc = DateTime.UtcNow
            });

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}