using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.UseCases;

public interface IPaintUseCase
{
    Task<PaintCalculationResultDto> CalculateAsync(
        PaintQuickInputDto input,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken = default);

    Task<PaintComparisonContext?> GetComparisonContextAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);

    Task<PaintComparisonExecution?> CompareAsync(
        Guid analysisId,
        PaintProductAlternativeDto firstProduct,
        PaintProductAlternativeDto secondProduct,
        CancellationToken cancellationToken = default);
}

public sealed record PaintComparisonContext(
    decimal NetAreaSquareMeters,
    decimal ReferenceLiters);

public sealed record PaintComparisonExecution(
    PaintComparisonContext Context,
    PaintComparisonResultDto Result,
    string PublicToken);

public sealed class PaintUseCase : IPaintUseCase
{
    private const string LegalVersion = "1.0.0";

    private readonly IPaintCalculator _calculator;
    private readonly IPaintAnalysisStore _store;
    private readonly IPaintAnalysisReader _reader;
    private readonly IPaintProductComparer _comparer;
    private readonly IPaintComparisonStore _comparisonStore;
    private readonly ISharedResultService _sharedResultService;
    private readonly IAnonymousAnalyticsService _analytics;

    public PaintUseCase(
        IPaintCalculator calculator,
        IPaintAnalysisStore store,
        IPaintAnalysisReader reader,
        IPaintProductComparer comparer,
        IPaintComparisonStore comparisonStore,
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

    public async Task<PaintCalculationResultDto> CalculateAsync(
        PaintQuickInputDto input,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken = default)
    {
        var analysisId = Guid.NewGuid();
        var domainInput = new PaintInput
        {
            AnalysisId = analysisId,
            LengthMeters = input.LengthMeters,
            WidthMeters = input.WidthMeters,
            HeightMeters = input.HeightMeters,
            IncludeCeiling = input.IncludeCeiling,
            DoorCount = input.DoorCount,
            WindowCount = input.WindowCount,
            CoatCount = input.CoatCount,
            SurfaceCondition = input.SurfaceCondition,
            WastePercentage = 10m
        };

        var result = _calculator.Calculate(domainInput);
        await _store.SaveAsync(domainInput, result, AnalysisMode.Quick, cancellationToken);
        await RecordConsentsAsync(analysisId, allowAnonymousAnalytics, allowRadarData, cancellationToken);
        await _analytics.RecordEventAsync(
            analysisId,
            AnalyticsEventType.PaintCalculationCompleted,
            AnalysisMode.Quick,
            cancellationToken);

        return new PaintCalculationResultDto
        {
            AnalysisId = analysisId,
            NetAreaSquareMeters = result.NetAreaSquareMeters,
            AdjustedAreaSquareMeters = result.AdjustedAreaSquareMeters,
            ReferenceLiters = result.ReferenceLiters,
            CoatCount = domainInput.CoatCount,
            ConfidenceLevel = result.ConfidenceLevel,
            RequiresProfessionalReview = result.RequiresProfessionalReview,
            Explanation = "El cálculo descuenta puertas y ventanas estándar, aplica el estado de la superficie, las manos elegidas y un margen de desperdicio del 10 %."
        };
    }

    public async Task<PaintComparisonContext?> GetComparisonContextAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var result = await _reader.GetCalculationResultAsync(analysisId, cancellationToken);
        return result is null
            ? null
            : new PaintComparisonContext(result.NetAreaSquareMeters, result.ReferenceLiters);
    }

    public async Task<PaintComparisonExecution?> CompareAsync(
        Guid analysisId,
        PaintProductAlternativeDto firstProduct,
        PaintProductAlternativeDto secondProduct,
        CancellationToken cancellationToken = default)
    {
        var calculation = await _reader.GetCalculationResultAsync(analysisId, cancellationToken);
        var input = await _reader.GetInputAsync(analysisId, cancellationToken);
        if (calculation is null || input is null)
        {
            return null;
        }

        var result = _comparer.Compare(input, calculation, firstProduct, secondProduct);
        await _comparisonStore.SaveAsync(analysisId, firstProduct, secondProduct, result, cancellationToken);
        await _analytics.RecordEventAsync(
            analysisId,
            AnalyticsEventType.PaintComparisonCompleted,
            null,
            cancellationToken);

        var token = await _sharedResultService.CreateOrGetTokenAsync(analysisId, cancellationToken);
        return new PaintComparisonExecution(
            new PaintComparisonContext(calculation.NetAreaSquareMeters, calculation.ReferenceLiters),
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
