using ElegiBien.Application.DTOs;

namespace ElegiBien.Web.Models.Paint;

public class PaintComparisonViewModel
{
    public Guid AnalysisId { get; set; }
    public decimal NetAreaSquareMeters { get; set; }
    public decimal ReferenceLiters { get; set; }
    public PaintProductAlternativeDto FirstProduct { get; set; } = new();
    public PaintProductAlternativeDto SecondProduct { get; set; } = new();
    public PaintComparisonResultDto? Result { get; set; }
    public string? ShareUrl { get; set; }
}
