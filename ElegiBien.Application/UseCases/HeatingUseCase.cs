using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.UseCases;

public interface IHeatingUseCase
{
    Task<HeatingCalculationResultDto> CalculateAsync(
        HeatingQuickInputDto input,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken = default);

    Task<HeatingComparisonContext?> GetComparisonContextAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);

    Task<HeatingComparisonExecution?> CompareAsync(
        Guid analysisId,
        HeatingProductAlternativeDto firstProduct,
        HeatingProductAlternativeDto secondProduct,
        CancellationToken cancellationToken = default);
}

public sealed record HeatingComparisonContext(
    decimal RecommendedMinimumWatts,
    decimal RecommendedMaximumWatts);

public sealed record HeatingComparisonExecution(
    HeatingComparisonContext Context,
    HeatingComparisonResultDto Result,
    string PublicToken);

public sealed class HeatingUseCase : IHeatingUseCase
{
    private const string LegalVersion = "1.0.0";

    private readonly IHeatingCalculator _calculator;
    private readonly IHeatingAnalysisStore _store;
    private readonly IHeatingAnalysisReader _reader;
    private readonly IHeatingProductComparer _comparer;
    private readonly IHeatingComparisonStore _comparisonStore;
    private readonly ISharedResultService _sharedResultService;
    private readonly IAnonymousAnalyticsService _analytics;

    public HeatingUseCase(
        IHeatingCalculator calculator,
        IHeatingAnalysisStore store,
        IHeatingAnalysisReader reader,
        IHeatingProductComparer comparer,
        IHeatingComparisonStore comparisonStore,
        ISharedResultService sharedResultService,
        IAnonymousAnalyticsService analytics)
    {
        _calculator = calculator;
        _store = store;
        _reader = reader;
        _comparer = comparer;
        _comparisonStore = comparisonStore;
        _sharedResultService = sharedResultService;
        _analytics = analytics;
    }

    public async Task<HeatingCalculationResultDto> CalculateAsync(
        HeatingQuickInputDto input,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken = default)
    {
        var analysisId = Guid.NewGuid();
        var domainInput = new HeatingInput
        {
            AnalysisId = analysisId,
            LengthMeters = input.LengthMeters,
            WidthMeters = input.WidthMeters,
            HeightMeters = input.HeightMeters,
            IsHeightAssumed = false,
            ClimateZone = input.ClimateZone,
            InsulationLevel = input.InsulationLevel,
            ExteriorWallsCount = input.ExteriorWallsCount,
            WindowExposure = input.WindowExposure,
            IsOpenToAnotherSpace = input.IsOpenToAnotherSpace
        };

        var result = _calculator.Calculate(domainInput);
        await _store.SaveAsync(domainInput, result, AnalysisMode.Quick, cancellationToken);
        await RecordConsentsAsync(analysisId, allowAnonymousAnalytics, allowRadarData, cancellationToken);
        await _analytics.RecordEventAsync(
            analysisId,
            AnalyticsEventType.HeatingCalculationCompleted,
            AnalysisMode.Quick,
            cancellationToken);

        return new HeatingCalculationResultDto
        {
            AnalysisId = analysisId,
            SurfaceSquareMeters = result.SurfaceSquareMeters,
            VolumeCubicMeters = result.VolumeCubicMeters,
            RecommendedMinimumWatts = result.RecommendedMinimumWatts,
            RecommendedMaximumWatts = result.RecommendedMaximumWatts,
            IdealPowerWatts = result.IdealPowerWatts,
            IdealPowerKcalPerHour = result.IdealPowerKcalPerHour,
            ConfidenceLevel = result.ConfidenceLevel,
            RequiresProfessionalReview = result.RequiresProfessionalReview
        };
    }

    public async Task<HeatingComparisonContext?> GetComparisonContextAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var result = await _reader.GetCalculationResultAsync(analysisId, cancellationToken);
        return result is null
            ? null
            : new HeatingComparisonContext(
                result.RecommendedMinimumWatts,
                result.RecommendedMaximumWatts);
    }

    public async Task<HeatingComparisonExecution?> CompareAsync(
        Guid analysisId,
        HeatingProductAlternativeDto firstProduct,
        HeatingProductAlternativeDto secondProduct,
        CancellationToken cancellationToken = default)
    {
        var calculation = await _reader.GetCalculationResultAsync(analysisId, cancellationToken);
        if (calculation is null)
        {
            return null;
        }

        var result = _comparer.Compare(calculation, firstProduct, secondProduct);
        await _comparisonStore.SaveAsync(analysisId, firstProduct, secondProduct, result, cancellationToken);
        await _analytics.RecordEventAsync(
            analysisId,
            AnalyticsEventType.HeatingComparisonCompleted,
            null,
            cancellationToken);

        var token = await _sharedResultService.CreateOrGetTokenAsync(analysisId, cancellationToken);
        return new HeatingComparisonExecution(
            new HeatingComparisonContext(
                calculation.RecommendedMinimumWatts,
                calculation.RecommendedMaximumWatts),
            result,
            token);
    }

    private async Task RecordConsentsAsync(
        Guid analysisId,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken)
    {
        await _analytics.RecordConsentAsync(
            analysisId,
            ConsentType.AnonymousAnalytics,
            allowAnonymousAnalytics,
            LegalVersion,
            cancellationToken);
        await _analytics.RecordConsentAsync(
            analysisId,
            ConsentType.RadarData,
            allowRadarData,
            LegalVersion,
            cancellationToken);
    }
}
