using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class PaintCalculationResultDto
{
    public Guid AnalysisId { get; set; }
    public decimal NetAreaSquareMeters { get; set; }
    public decimal AdjustedAreaSquareMeters { get; set; }
    public decimal ReferenceLiters { get; set; }
    public int CoatCount { get; set; }
    public ConfidenceLevel ConfidenceLevel { get; set; }
    public bool RequiresProfessionalReview { get; set; }
    public string Explanation { get; set; } = string.Empty;
}
