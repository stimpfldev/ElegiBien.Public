using ElegiBien.Application.Interfaces;
using ElegiBien.Application.UseCases;
using ElegiBien.Web.Models.AirConditioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ElegiBien.Web.Controllers;

public class AirConditioningController : Controller
{
    private readonly IAirConditioningUseCase _useCase;
    private readonly ISharedResultService _sharedResultService;

    public AirConditioningController(
        IAirConditioningUseCase useCase,
        ISharedResultService sharedResultService)
    {
        _useCase = useCase;
        _sharedResultService = sharedResultService;
    }

    [HttpGet]
    public IActionResult Index() => View(new AirConditioningQuickViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("public-forms")]
    public async Task<IActionResult> Index(
        AirConditioningQuickViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.Result = await _useCase.CalculateAsync(
            model.Input,
            model.AllowAnonymousAnalytics,
            model.AllowRadarData,
            cancellationToken);

        var token = await _sharedResultService.CreateOrGetTokenAsync(
            model.Result.AnalysisId,
            cancellationToken);

        model.ShareUrl = Url.Action(
            "Result",
            "Shared",
            new { token },
            Request.Scheme);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Compare(Guid id, CancellationToken cancellationToken)
    {
        var context = await _useCase.GetComparisonContextAsync(id, cancellationToken);
        if (context is null)
        {
            return NotFound();
        }

        return View(new AirConditioningComparisonViewModel
        {
            AnalysisId = id,
            RecommendedMinimumFrigories = context.RecommendedMinimumFrigories,
            RecommendedMaximumFrigories = context.RecommendedMaximumFrigories
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("public-forms")]
    public async Task<IActionResult> Compare(
        AirConditioningComparisonViewModel model,
        CancellationToken cancellationToken)
    {
        var context = await _useCase.GetComparisonContextAsync(model.AnalysisId, cancellationToken);
        if (context is null)
        {
            return NotFound();
        }

        model.RecommendedMinimumFrigories = context.RecommendedMinimumFrigories;
        model.RecommendedMaximumFrigories = context.RecommendedMaximumFrigories;
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var execution = await _useCase.CompareAsync(
            model.AnalysisId,
            model.FirstProduct,
            model.SecondProduct,
            cancellationToken);

        if (execution is null)
        {
            return NotFound();
        }

        model.Result = execution.Result;
        model.ShareUrl = Url.Action(
            "Result",
            "Shared",
            new { token = execution.PublicToken },
            Request.Scheme);

        return View(model);
    }
}
