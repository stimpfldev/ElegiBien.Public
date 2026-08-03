using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class DimensioningResult
{
    public Guid DimensioningResultId { get; set; } = Guid.NewGuid();

    public Guid AnalysisId { get; set; }

    public decimal VolumeCubicMeters { get; set; }

    public decimal BaseFrigories { get; set; }

    public decimal AdjustmentFrigories { get; set; }

    public decimal EstimatedFrigories { get; set; }

    public decimal RecommendedMinimumFrigories { get; set; }

    public decimal RecommendedMaximumFrigories { get; set; }

    public decimal IdealFrigories { get; set; }

    public ConfidenceLevel ConfidenceLevel { get; set; }

    public bool RequiresProfessionalReview { get; set; }

    public Analysis Analysis { get; set; } = null!;
}