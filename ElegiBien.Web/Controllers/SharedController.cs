using ElegiBien.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElegiBien.Web.Controllers;

public class SharedController : Controller
{
    private readonly ISharedResultService _sharedResultService;
    private readonly ISharedAirConditioningResultReader _airResultReader;
    private readonly ISharedPaintResultReader _paintResultReader;
    private readonly ISharedFlooringResultReader _flooringResultReader;
    private readonly ISharedHeatingResultReader _heatingResultReader;

    public SharedController(
        ISharedResultService sharedResultService,
        ISharedAirConditioningResultReader airResultReader,
        ISharedPaintResultReader paintResultReader,
        ISharedFlooringResultReader flooringResultReader,
        ISharedHeatingResultReader heatingResultReader)
    {
        _sharedResultService = sharedResultService;
        _airResultReader = airResultReader;
        _paintResultReader = paintResultReader;
        _flooringResultReader = flooringResultReader;
        _heatingResultReader = heatingResultReader;
    }

    [HttpGet]
    public async Task<IActionResult> Result(
        string token,
        CancellationToken cancellationToken)
    {
        var analysisId =
            await _sharedResultService.GetAnalysisIdAsync(
                token,
                cancellationToken);

        if (!analysisId.HasValue)
        {
            return NotFound();
        }

        var result =
            await _airResultReader.GetAsync(
                analysisId.Value,
                cancellationToken);

        return result is null
            ? NotFound()
            : View(result);
    }

    [HttpGet]
    public async Task<IActionResult> PaintResult(
        string token,
        CancellationToken cancellationToken)
    {
        var analysisId =
            await _sharedResultService.GetAnalysisIdAsync(
                token,
                cancellationToken);

        if (!analysisId.HasValue)
        {
            return NotFound();
        }

        var result =
            await _paintResultReader.GetAsync(
                analysisId.Value,
                cancellationToken);

        return result is null
            ? NotFound()
            : View(result);
    }

    [HttpGet]
    public async Task<IActionResult> FlooringResult(
        string token,
        CancellationToken cancellationToken)
    {
        var analysisId =
            await _sharedResultService.GetAnalysisIdAsync(
                token,
                cancellationToken);

        if (!analysisId.HasValue)
        {
            return NotFound();
        }

        var result =
            await _flooringResultReader.GetAsync(
                analysisId.Value,
                cancellationToken);

        return result is null
            ? NotFound()
            : View(result);
    }

    [HttpGet]
    public async Task<IActionResult> HeatingResult(
        string token,
        CancellationToken cancellationToken)
    {
        var analysisId =
            await _sharedResultService.GetAnalysisIdAsync(
                token,
                cancellationToken);

        if (!analysisId.HasValue)
        {
            return NotFound();
        }

        var result =
            await _heatingResultReader.GetAsync(
                analysisId.Value,
                cancellationToken);

        return result is null
            ? NotFound()
            : View(result);
    }
}
