using ElegiBien.Application.UseCases;
using ElegiBien.Web.Models.Paint;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ElegiBien.Web.Controllers;

public class PaintController : Controller
{
    private readonly IPaintUseCase _useCase;

    public PaintController(IPaintUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public IActionResult Index() => View(new PaintQuickViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("public-forms")]
    public async Task<IActionResult> Index(
        PaintQuickViewModel model,
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

        return View(new PaintComparisonViewModel
        {
            AnalysisId = id,
            NetAreaSquareMeters = context.NetAreaSquareMeters,
            ReferenceLiters = context.ReferenceLiters
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("public-forms")]
    public async Task<IActionResult> Compare(
        PaintComparisonViewModel model,
        CancellationToken cancellationToken)
    {
        var context = await _useCase.GetComparisonContextAsync(model.AnalysisId, cancellationToken);
        if (context is null)
        {
            return NotFound();
        }

        model.NetAreaSquareMeters = context.NetAreaSquareMeters;
        model.ReferenceLiters = context.ReferenceLiters;
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
            "PaintResult",
            "Shared",
            new { token = execution.PublicToken },
            Request.Scheme);

        return View(model);
    }
}
