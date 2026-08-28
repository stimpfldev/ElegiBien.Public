using ElegiBien.Application.Interfaces;
using ElegiBien.Application.UseCases;
using ElegiBien.Web.Models.Heating;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ElegiBien.Web.Controllers;

public class HeatingController : Controller
{
    private readonly IHeatingUseCase _useCase;
    private readonly ISharedResultService _sharedResultService;

    public HeatingController(
        IHeatingUseCase useCase,
        ISharedResultService sharedResultService)
    {
        _useCase = useCase;
        _sharedResultService = sharedResultService;
    }

    [HttpGet]
    public IActionResult Index() => View(new HeatingQuickViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("public-forms")]
    public async Task<IActionResult> Index(
        HeatingQuickViewModel model,
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
            "HeatingResult",
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

        return View(new HeatingComparisonViewModel
        {
            AnalysisId = id,
            RecommendedMinimumWatts = context.RecommendedMinimumWatts,
            RecommendedMaximumWatts = context.RecommendedMaximumWatts
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("public-forms")]
    public async Task<IActionResult> Compare(
        HeatingComparisonViewModel model,
        CancellationToken cancellationToken)
    {
        var context = await _useCase.GetComparisonContextAsync(model.AnalysisId, cancellationToken);
        if (context is null)
        {
            return NotFound();
        }

        model.RecommendedMinimumWatts = context.RecommendedMinimumWatts;
        model.RecommendedMaximumWatts = context.RecommendedMaximumWatts;
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
            "HeatingResult",
            "Shared",
            new { token = execution.PublicToken },
            Request.Scheme);

        return View(model);
    }
}
