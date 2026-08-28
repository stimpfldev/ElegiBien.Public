using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class SharedAirConditioningResultDto
{
    public decimal RecommendedMinimumFrigories { get; set; }

    public decimal RecommendedMaximumFrigories { get; set; }

    public decimal IdealFrigories { get; set; }

    public decimal SurfaceSquareMeters { get; set; }

    public decimal VolumeCubicMeters { get; set; }

    public ConfidenceLevel ConfidenceLevel { get; set; }

    public bool RequiresProfessionalReview { get; set; }

    public List<ProductScoreResultDto> Products { get; set; } = [];

    public string Recommendation { get; set; } = string.Empty;
}
