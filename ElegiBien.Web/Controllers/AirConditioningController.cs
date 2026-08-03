using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Web.Models.AirConditioning;
using Microsoft.AspNetCore.Mvc;

namespace ElegiBien.Web.Controllers;

public class AirConditioningController : Controller
{
    private readonly IAirConditioningCalculator _calculator;
    private readonly IAirConditioningAnalysisStore _analysisStore;

    public AirConditioningController(
        IAirConditioningCalculator calculator,
        IAirConditioningAnalysisStore analysisStore)
    {
        _calculator = calculator;
        _analysisStore = analysisStore;
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

        model.Result = new AirConditioningResultDto
        {
            AnalysisId = analysisId,
            RecommendedMinimumFrigories =
                result.RecommendedMinimumFrigories,
            RecommendedMaximumFrigories =
                result.RecommendedMaximumFrigories,
            IdealFrigories = result.IdealFrigories,
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
}