using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.UseCases;

public interface IFlooringUseCase
{
    Task<FlooringCalculationResultDto> CalculateAsync(
        FlooringQuickInputDto input,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken = default);

    Task<FlooringComparisonContext?> GetComparisonContextAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);

    Task<FlooringComparisonExecution?> CompareAsync(
        Guid analysisId,
        FlooringProductAlternativeDto firstProduct,
        FlooringProductAlternativeDto secondProduct,
        CancellationToken cancellationToken = default);
}

public sealed record FlooringComparisonContext(decimal RequiredAreaSquareMeters);

public sealed record FlooringComparisonExecution(
    FlooringComparisonContext Context,
    FlooringComparisonResultDto Result,
    string PublicToken);

public sealed class FlooringUseCase : IFlooringUseCase
{
    private const string LegalVersion = "1.0.0";

    private readonly IFlooringCalculator _calculator;
    private readonly IFlooringAnalysisStore _store;
    private readonly IFlooringAnalysisReader _reader;
    private readonly IFlooringProductComparer _comparer;
    private readonly IFlooringComparisonStore _comparisonStore;
    private readonly ISharedResultService _sharedResultService;
    private readonly IAnonymousAnalyticsService _analytics;

    public FlooringUseCase(
        IFlooringCalculator calculator,
        IFlooringAnalysisStore store,
        IFlooringAnalysisReader reader,
        IFlooringProductComparer comparer,
        IFlooringComparisonStore comparisonStore,
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

    public async Task<FlooringCalculationResultDto> CalculateAsync(
        FlooringQuickInputDto input,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken = default)
    {
        var analysisId = Guid.NewGuid();
        var domainInput = new FlooringInput
        {
            AnalysisId = analysisId,
            LengthMeters = input.LengthMeters,
            WidthMeters = input.WidthMeters,
            InstallationPattern = input.InstallationPattern,
            WastePercentage = input.WastePercentage
        };

        var result = _calculator.Calculate(domainInput);
        await _store.SaveAsync(domainInput, result, AnalysisMode.Quick, cancellationToken);
        await RecordConsentsAsync(analysisId, allowAnonymousAnalytics, allowRadarData, cancellationToken);
        await _analytics.RecordEventAsync(
            analysisId,
            AnalyticsEventType.FlooringCalculationCompleted,
            AnalysisMode.Quick,
            cancellationToken);

        return new FlooringCalculationResultDto
        {
            AnalysisId = analysisId,
            TotalAreaSquareMeters = result.TotalAreaSquareMeters,
            WastePercentage = result.WastePercentage,
            WasteAreaSquareMeters = result.WasteAreaSquareMeters,
            RequiredAreaSquareMeters = result.RequiredAreaSquareMeters,
            ConfidenceLevel = result.ConfidenceLevel,
            RequiresProfessionalReview = result.RequiresProfessionalReview,
            Explanation = "La estimación calcula la superficie rectangular y agrega el porcentaje de desperdicio elegido según el tipo de colocación."
        };
    }

    public async Task<FlooringComparisonContext?> GetComparisonContextAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var result = await _reader.GetCalculationResultAsync(analysisId, cancellationToken);
        return result is null
            ? null
            : new FlooringComparisonContext(result.RequiredAreaSquareMeters);
    }

    public async Task<FlooringComparisonExecution?> CompareAsync(
        Guid analysisId,
        FlooringProductAlternativeDto firstProduct,
        FlooringProductAlternativeDto secondProduct,
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
            AnalyticsEventType.FlooringComparisonCompleted,
            null,
            cancellationToken);

        var token = await _sharedResultService.CreateOrGetTokenAsync(analysisId, cancellationToken);
        return new FlooringComparisonExecution(
            new FlooringComparisonContext(calculation.RequiredAreaSquareMeters),
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
