using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.UseCases;

public interface IAirConditioningUseCase
{
    Task<AirConditioningResultDto> CalculateAsync(
        AirConditioningQuickInputDto input,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken = default);

    Task<AirConditioningComparisonContext?> GetComparisonContextAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);

    Task<AirConditioningComparisonExecution?> CompareAsync(
        Guid analysisId,
        ProductAlternativeDto firstProduct,
        ProductAlternativeDto secondProduct,
        CancellationToken cancellationToken = default);
}

public sealed record AirConditioningComparisonContext(
    decimal RecommendedMinimumFrigories,
    decimal RecommendedMaximumFrigories);

public sealed record AirConditioningComparisonExecution(
    AirConditioningComparisonContext Context,
    ProductComparisonResultDto Result,
    string PublicToken);

public sealed class AirConditioningUseCase : IAirConditioningUseCase
{
    private const string LegalVersion = "1.0.0";

    private readonly IAirConditioningCalculator _calculator;
    private readonly IAirConditioningAnalysisStore _analysisStore;
    private readonly IAirConditioningAnalysisReader _analysisReader;
    private readonly IAirConditioningProductComparer _productComparer;
    private readonly IAirConditioningComparisonStore _comparisonStore;
    private readonly ISharedResultService _sharedResultService;
    private readonly IAnonymousAnalyticsService _analyticsService;

    public AirConditioningUseCase(
        IAirConditioningCalculator calculator,
        IAirConditioningAnalysisStore analysisStore,
        IAirConditioningAnalysisReader analysisReader,
        IAirConditioningProductComparer productComparer,
        IAirConditioningComparisonStore comparisonStore,
        ISharedResultService sharedResultService,
        IAnonymousAnalyticsService analyticsService)
    {
        _calculator = calculator;
        _analysisStore = analysisStore;
        _analysisReader = analysisReader;
        _productComparer = productComparer;
        _comparisonStore = comparisonStore;
        _sharedResultService = sharedResultService;
        _analyticsService = analyticsService;
    }

    public async Task<AirConditioningResultDto> CalculateAsync(
        AirConditioningQuickInputDto input,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken = default)
    {
        var analysisId = Guid.NewGuid();
        var domainInput = new AirConditioningInput
        {
            AnalysisId = analysisId,
            LengthMeters = input.LengthMeters,
            WidthMeters = input.WidthMeters,
            HeightMeters = 2.60m,
            IsHeightAssumed = true,
            PeopleCount = input.PeopleCount,
            SunExposure = input.SunExposure,
            ClimateZone = ClimateZone.Temperate,
            InsulationLevel = InsulationLevel.Normal,
            WindowExposure = WindowExposure.Normal,
            IsOpenToAnotherSpace = false,
            HasHighHeatEquipment = false
        };

        var result = _calculator.Calculate(domainInput);

        await _analysisStore.SaveAsync(domainInput, result, AnalysisMode.Quick, cancellationToken);
        await RecordConsentsAsync(analysisId, allowAnonymousAnalytics, allowRadarData, cancellationToken);
        await _analyticsService.RecordEventAsync(
            analysisId,
            AnalyticsEventType.DimensioningCompleted,
            AnalysisMode.Quick,
            cancellationToken);

        return new AirConditioningResultDto
        {
            AnalysisId = analysisId,
            RecommendedMinimumFrigories = result.RecommendedMinimumFrigories,
            RecommendedMaximumFrigories = result.RecommendedMaximumFrigories,
            IdealFrigories = result.IdealFrigories,
            SurfaceSquareMeters = domainInput.LengthMeters * domainInput.WidthMeters,
            VolumeCubicMeters = result.VolumeCubicMeters,
            ConfidenceLevel = result.ConfidenceLevel,
            RequiresProfessionalReview = result.RequiresProfessionalReview,
            Explanation = "El cálculo utiliza las dimensiones del ambiente, una altura estándar de 2,60 metros, la cantidad de personas y la exposición solar."
        };
    }

    public async Task<AirConditioningComparisonContext?> GetComparisonContextAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var result = await _analysisReader.GetDimensioningResultAsync(analysisId, cancellationToken);
        return result is null
            ? null
            : new AirConditioningComparisonContext(
                result.RecommendedMinimumFrigories,
                result.RecommendedMaximumFrigories);
    }

    public async Task<AirConditioningComparisonExecution?> CompareAsync(
        Guid analysisId,
        ProductAlternativeDto firstProduct,
        ProductAlternativeDto secondProduct,
        CancellationToken cancellationToken = default)
    {
        var dimensioningResult = await _analysisReader.GetDimensioningResultAsync(analysisId, cancellationToken);
        if (dimensioningResult is null)
        {
            return null;
        }

        var result = _productComparer.Compare(dimensioningResult, firstProduct, secondProduct);
        await _comparisonStore.SaveAsync(analysisId, firstProduct, secondProduct, result, cancellationToken);
        await _analyticsService.RecordEventAsync(
            analysisId,
            AnalyticsEventType.ComparisonCompleted,
            null,
            cancellationToken);

        var token = await _sharedResultService.CreateOrGetTokenAsync(analysisId, cancellationToken);
        return new AirConditioningComparisonExecution(
            new AirConditioningComparisonContext(
                dimensioningResult.RecommendedMinimumFrigories,
                dimensioningResult.RecommendedMaximumFrigories),
            result,
            token);
    }

    private async Task RecordConsentsAsync(
        Guid analysisId,
        bool allowAnonymousAnalytics,
        bool allowRadarData,
        CancellationToken cancellationToken)
    {
        await _analyticsService.RecordConsentAsync(
            analysisId,
            ConsentType.AnonymousAnalytics,
            allowAnonymousAnalytics,
            LegalVersion,
            cancellationToken);
        await _analyticsService.RecordConsentAsync(
            analysisId,
            ConsentType.RadarData,
            allowRadarData,
            LegalVersion,
            cancellationToken);
    }
}
