using ElegiBien.Application.DTOs;

namespace ElegiBien.Web.Models.Flooring;

public class FlooringQuickViewModel
{
    public FlooringQuickInputDto Input { get; set; } = new();
    public bool AllowAnonymousAnalytics { get; set; }
    public bool AllowRadarData { get; set; }
    public FlooringCalculationResultDto? Result { get; set; }
    public string? ShareUrl { get; set; }
}
