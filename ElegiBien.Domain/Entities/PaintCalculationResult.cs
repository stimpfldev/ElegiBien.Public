using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class PaintCalculationResult
{
    public Guid PaintCalculationResultId { get; set; } = Guid.NewGuid();
    public Guid AnalysisId { get; set; }
    public decimal WallAreaSquareMeters { get; set; }
    public decimal CeilingAreaSquareMeters { get; set; }
    public decimal DeductedAreaSquareMeters { get; set; }
    public decimal NetAreaSquareMeters { get; set; }
    public decimal AdjustedAreaSquareMeters { get; set; }
    public decimal ReferenceCoverageSquareMetersPerLiter { get; set; }
    public decimal ReferenceLiters { get; set; }
    public ConfidenceLevel ConfidenceLevel { get; set; }
    public bool RequiresProfessionalReview { get; set; }
    public Analysis Analysis { get; set; } = null!;
}
