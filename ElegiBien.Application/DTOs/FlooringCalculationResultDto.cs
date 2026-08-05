using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class FlooringCalculationResultDto
{
    public Guid AnalysisId { get; set; }
    public decimal TotalAreaSquareMeters { get; set; }
    public decimal WastePercentage { get; set; }
    public decimal WasteAreaSquareMeters { get; set; }
    public decimal RequiredAreaSquareMeters { get; set; }
    public ConfidenceLevel ConfidenceLevel { get; set; }
    public bool RequiresProfessionalReview { get; set; }
    public string Explanation { get; set; } = string.Empty;
}
