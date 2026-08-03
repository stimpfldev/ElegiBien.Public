using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Interfaces;

public interface IAnonymousAnalyticsService
{
    Task RecordConsentAsync(
        Guid analysisId,
        ConsentType consentType,
        bool isGranted,
        string legalVersion,
        CancellationToken cancellationToken = default);

    Task RecordEventAsync(
        Guid analysisId,
        AnalyticsEventType eventType,
        AnalysisMode? mode,
        CancellationToken cancellationToken = default);
}