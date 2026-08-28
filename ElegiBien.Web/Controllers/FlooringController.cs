using ElegiBien.Application.Interfaces;
using ElegiBien.Application.UseCases;
using ElegiBien.Web.Models.Flooring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ElegiBien.Web.Controllers;

public class FlooringController : Controller
{
    private readonly IFlooringUseCase _useCase;
    private readonly ISharedResultService _sharedResultService;

    public FlooringController(
        IFlooringUseCase useCase,
        ISharedResultService sharedResultService)
    {
        _useCase = useCase;
        _sharedResultService = sharedResultService;
    }

    [HttpGet]
    public IActionResult Index() => View(new FlooringQuickViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("public-forms")]
    public async Task<IActionResult> Index(
        FlooringQuickViewModel model,
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
            "FlooringResult",
            "Shared",
            new { token },
            Request.Scheme);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Compare(Guid? id, CancellationToken cancellationToken)
    {
        if (!id.HasValue || id.Value == Guid.Empty)
        {
            return View(new FlooringComparisonViewModel());
        }

        var context = await _useCase.GetComparisonContextAsync(id.Value, cancellationToken);
        if (context is null)
        {
            return NotFound();
        }

        return View(new FlooringComparisonViewModel
        {
            AnalysisId = id.Value,
            RequiredAreaSquareMeters = context.RequiredAreaSquareMeters
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("public-forms")]
    public async Task<IActionResult> Compare(
        FlooringComparisonViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.AnalysisId == Guid.Empty)
        {
            var createdContext = await _useCase.CreateComparisonContextAsync(
                model.RequiredAreaSquareMeters,
                cancellationToken);

            model.AnalysisId = createdContext.AnalysisId;
            model.RequiredAreaSquareMeters = createdContext.RequiredAreaSquareMeters;
        }
        else
        {
            var context = await _useCase.GetComparisonContextAsync(model.AnalysisId, cancellationToken);
            if (context is null)
            {
                return NotFound();
            }

            model.RequiredAreaSquareMeters = context.RequiredAreaSquareMeters;
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
            "FlooringResult",
            "Shared",
            new { token = execution.PublicToken },
            Request.Scheme);

        return View(model);
    }
}
