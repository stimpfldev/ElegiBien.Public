using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class HeatingCalculationResultDto
{
    public Guid AnalysisId { get; set; }

    public decimal SurfaceSquareMeters { get; set; }

    public decimal VolumeCubicMeters { get; set; }

    public decimal RecommendedMinimumWatts { get; set; }

    public decimal RecommendedMaximumWatts { get; set; }

    public decimal IdealPowerWatts { get; set; }

    public decimal IdealPowerKcalPerHour { get; set; }

    public ConfidenceLevel ConfidenceLevel { get; set; }

    public bool RequiresProfessionalReview { get; set; }
}
