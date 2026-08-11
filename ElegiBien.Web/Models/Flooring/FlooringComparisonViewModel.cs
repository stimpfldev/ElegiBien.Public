using ElegiBien.Application.DTOs;

namespace ElegiBien.Web.Models.Flooring;

public class FlooringComparisonViewModel
{
    public Guid AnalysisId { get; set; }
    public decimal RequiredAreaSquareMeters { get; set; }
    public FlooringProductAlternativeDto FirstProduct { get; set; } = new();
    public FlooringProductAlternativeDto SecondProduct { get; set; } = new();
    public FlooringComparisonResultDto? Result { get; set; }
    public string? ShareUrl { get; set; }
}
