using ElegiBien.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElegiBien.Web.Controllers;

public class SharedController : Controller
{
    private readonly ISharedResultService _sharedResultService;
    private readonly ISharedAirConditioningResultReader _resultReader;

    public SharedController(
        ISharedResultService sharedResultService,
        ISharedAirConditioningResultReader resultReader)
    {
        _sharedResultService = sharedResultService;
        _resultReader = resultReader;
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

        var result = await _resultReader.GetAsync(
            analysisId.Value,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return View(result);
    }
}