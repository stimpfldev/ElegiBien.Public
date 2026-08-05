using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class HeatingCalculationResult
{
    public Guid HeatingCalculationResultId { get; set; } = Guid.NewGuid();

    public Guid AnalysisId { get; set; }

    public decimal SurfaceSquareMeters { get; set; }

    public decimal VolumeCubicMeters { get; set; }

    public decimal BasePowerWatts { get; set; }

    public decimal AdjustmentPowerWatts { get; set; }

    public decimal EstimatedPowerWatts { get; set; }

    public decimal RecommendedMinimumWatts { get; set; }

    public decimal RecommendedMaximumWatts { get; set; }

    public decimal IdealPowerWatts { get; set; }

    public decimal IdealPowerKcalPerHour { get; set; }

    public ConfidenceLevel ConfidenceLevel { get; set; }

    public bool RequiresProfessionalReview { get; set; }

    public Analysis Analysis { get; set; } = null!;
}
