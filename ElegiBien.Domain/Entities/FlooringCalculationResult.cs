using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class FlooringCalculationResult
{
    public Guid FlooringCalculationResultId { get; set; } = Guid.NewGuid();
    public Guid AnalysisId { get; set; }
    public decimal TotalAreaSquareMeters { get; set; }
    public decimal WastePercentage { get; set; }
    public decimal WasteAreaSquareMeters { get; set; }
    public decimal RequiredAreaSquareMeters { get; set; }
    public ConfidenceLevel ConfidenceLevel { get; set; }
    public bool RequiresProfessionalReview { get; set; }
    public Analysis Analysis { get; set; } = null!;
}
