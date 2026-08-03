using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Web.Models.AirConditioning;
using Microsoft.AspNetCore.Mvc;

namespace ElegiBien.Web.Controllers;

public class AirConditioningController : Controller
{
    private const string LegalVersion = "1.0";

    private readonly IAirConditioningCalculator _calculator;
    private readonly IAirConditioningAnalysisStore _analysisStore;
    private readonly IAirConditioningAnalysisReader _analysisReader;
    private readonly IAirConditioningProductComparer _productComparer;
    private readonly IAirConditioningComparisonStore _comparisonStore;
    private readonly ISharedResultService _sharedResultService;
    private readonly IAnonymousAnalyticsService _analyticsService;

    public AirConditioningController(
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

    [HttpGet]
    public IActionResult Index()
    {
        return View(new AirConditioningQuickViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        AirConditioningQuickViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var analysisId = Guid.NewGuid();

        var input = new AirConditioningInput
        {
            AnalysisId = analysisId,
            LengthMeters = model.Input.LengthMeters,
            WidthMeters = model.Input.WidthMeters,
            HeightMeters = 2.60m,
            IsHeightAssumed = true,
            PeopleCount = model.Input.PeopleCount,
            SunExposure = model.Input.SunExposure,
            ClimateZone = ClimateZone.Temperate,
            InsulationLevel = InsulationLevel.Normal,
            WindowExposure = WindowExposure.Normal,
            IsOpenToAnotherSpace = false,
            HasHighHeatEquipment = false
        };

        var result = _calculator.Calculate(input);

        await _analysisStore.SaveAsync(
            input,
            result,
            AnalysisMode.Quick,
            cancellationToken);

        await _analyticsService.RecordConsentAsync(
            analysisId,
            ConsentType.AnonymousAnalytics,
            model.AllowAnonymousAnalytics,
            LegalVersion,
            cancellationToken);

        await _analyticsService.RecordConsentAsync(
            analysisId,
            ConsentType.RadarData,
            model.AllowRadarData,
            LegalVersion,
            cancellationToken);

        await _analyticsService.RecordEventAsync(
            analysisId,
            AnalyticsEventType.DimensioningCompleted,
            AnalysisMode.Quick,
            cancellationToken);

        model.Result = new AirConditioningResultDto
        {
            AnalysisId = analysisId,
            RecommendedMinimumFrigories =
                result.RecommendedMinimumFrigories,
            RecommendedMaximumFrigories =
                result.RecommendedMaximumFrigories,
            IdealFrigories =
                result.IdealFrigories,
            SurfaceSquareMeters =
                input.LengthMeters * input.WidthMeters,
            VolumeCubicMeters =
                result.VolumeCubicMeters,
            ConfidenceLevel =
                result.ConfidenceLevel,
            RequiresProfessionalReview =
                result.RequiresProfessionalReview,
            Explanation =
                "El cálculo utiliza las dimensiones del ambiente, una altura estándar de 2,60 metros, la cantidad de personas y la exposición solar."
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Compare(
        Guid id,
        CancellationToken cancellationToken)
    {
        var dimensioningResult =
            await _analysisReader.GetDimensioningResultAsync(
                id,
                cancellationToken);

        if (dimensioningResult is null)
        {
            return NotFound();
        }

        return View(
            new AirConditioningComparisonViewModel
            {
                AnalysisId = id,
                RecommendedMinimumFrigories =
                    dimensioningResult.RecommendedMinimumFrigories,
                RecommendedMaximumFrigories =
                    dimensioningResult.RecommendedMaximumFrigories
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Compare(
        AirConditioningComparisonViewModel model,
        CancellationToken cancellationToken)
    {
        var dimensioningResult =
            await _analysisReader.GetDimensioningResultAsync(
                model.AnalysisId,
                cancellationToken);

        if (dimensioningResult is null)
        {
            return NotFound();
        }

        model.RecommendedMinimumFrigories =
            dimensioningResult.RecommendedMinimumFrigories;

        model.RecommendedMaximumFrigories =
            dimensioningResult.RecommendedMaximumFrigories;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.Result = _productComparer.Compare(
            dimensioningResult,
            model.FirstProduct,
            model.SecondProduct);

        await _comparisonStore.SaveAsync(
            model.AnalysisId,
            model.FirstProduct,
            model.SecondProduct,
            model.Result,
            cancellationToken);

        await _analyticsService.RecordEventAsync(
            model.AnalysisId,
            AnalyticsEventType.ComparisonCompleted,
            null,
            cancellationToken);

        var publicToken =
            await _sharedResultService.CreateOrGetTokenAsync(
                model.AnalysisId,
                cancellationToken);

        model.ShareUrl = Url.Action(
            action: "Result",
            controller: "Shared",
            values: new { token = publicToken },
            protocol: Request.Scheme);

        return View(model);
    }
}