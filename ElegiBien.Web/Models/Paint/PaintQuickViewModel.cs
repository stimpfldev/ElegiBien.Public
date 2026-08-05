using ElegiBien.Application.DTOs;

namespace ElegiBien.Web.Models.Paint;

public class PaintQuickViewModel
{
    public PaintQuickInputDto Input { get; set; } = new();
    public bool AllowAnonymousAnalytics { get; set; }
    public bool AllowRadarData { get; set; }
    public PaintCalculationResultDto? Result { get; set; }
}
